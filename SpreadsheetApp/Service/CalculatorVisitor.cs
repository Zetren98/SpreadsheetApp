using Antlr4.Runtime.Misc;
using SpreadsheetApp.Models;
using SpreadsheetApp.Service;
using System.Numerics;
using Cell = SpreadsheetApp.Models.Cell;

namespace SpreadsheetApp.Services
{
    public class CalculatorVisitor : LabCalculatorBaseVisitor<object>
    {
        private readonly Dictionary<string, Cell> _cells;

        public CalculatorVisitor(Dictionary<string, Cell> cells)
        {
            _cells = cells;
        }

        public override object VisitCompileUnit(LabCalculatorParser.CompileUnitContext context)
        {
            return Visit(context.expression());
        }

        public override object VisitBinaryAddSub(LabCalculatorParser.BinaryAddSubContext context)
        {
            var left = ToBigInt(Visit(context.additive(0)));
            var right = ToBigInt(Visit(context.additive(1)));
            var op = context.GetChild(1).GetText();

            if (op == "+") return left + right;
            if (op == "-") return left - right;
            return BigInteger.Zero;
        }

        public override object VisitUnaryNum(LabCalculatorParser.UnaryNumContext context)
        {
            var val = ToBigInt(Visit(context.unary()));
            if (context.MINUS() != null) return BigInteger.Negate(val);
            return val;
        }

        public override object VisitUnaryExpr(LabCalculatorParser.UnaryExprContext context)
        {
            return Visit(context.unary());
        }

        public override object VisitUnaryBool(LabCalculatorParser.UnaryBoolContext context)
        {
            return !ToBool(Visit(context.unary()));
        }

        public override object VisitAtomExpr(LabCalculatorParser.AtomExprContext context)
        {
            return Visit(context.atom());
        }

        public override object VisitParenExpr(LabCalculatorParser.ParenExprContext context)
        {
            return Visit(context.expression());
        }

        public override object VisitLogicEqv(LabCalculatorParser.LogicEqvContext context)
        {
            var left = Visit(context.logicOr(0));
            for (int i = 1; i < context.logicOr().Length; i++)
            {
                var right = Visit(context.logicOr(i));
                left = ToBool(left) == ToBool(right);
            }
            return left;
        }

        public override object VisitLogicOr(LabCalculatorParser.LogicOrContext context)
        {
            var left = Visit(context.logicAnd(0));
            for (int i = 1; i < context.logicAnd().Length; i++)
            {
                var right = Visit(context.logicAnd(i));
                left = ToBool(left) || ToBool(right);
            }
            return left;
        }

        public override object VisitLogicAnd(LabCalculatorParser.LogicAndContext context)
        {
            var left = Visit(context.comparison(0));
            for (int i = 1; i < context.comparison().Length; i++)
            {
                var right = Visit(context.comparison(i));
                left = ToBool(left) && ToBool(right);
            }
            return left;
        }

        public override object VisitComparison(LabCalculatorParser.ComparisonContext context)
        {
            var left = Visit(context.additive(0));
            if (context.additive().Length > 1)
            {
                var right = Visit(context.additive(1));
                var op = context.GetChild(1).GetText();

                if (left is BigInteger l && right is BigInteger r)
                {
                    if (op == "=") return l == r;
                    if (op == "<") return l < r;
                    if (op == ">") return l > r;
                }
                else
                {
                    if (op == "=") return left.Equals(right);
                    throw new ArgumentException("Type mismatch");
                }
            }
            return left;
        }

        public override object VisitConstant(LabCalculatorParser.ConstantContext context)
        {
            return BigInteger.Parse(context.GetText());
        }

        public override object VisitCellRef(LabCalculatorParser.CellRefContext context)
        {
            string address = context.GetText().ToUpper();
            if (_cells.ContainsKey(address))
            {
                string raw = _cells[address].Value;
                if (BigInteger.TryParse(raw, out BigInteger bi)) return bi;
                if (bool.TryParse(raw, out bool b)) return b;
            }
            return BigInteger.Zero;
        }

        public override object VisitBoolTrue(LabCalculatorParser.BoolTrueContext context)
        {
            return true;
        }

        public override object VisitBoolFalse(LabCalculatorParser.BoolFalseContext context)
        {
            return false;
        }

        private bool ToBool(object obj) => obj is bool b ? b : (obj is BigInteger i && i != 0);
        private BigInteger ToBigInt(object obj)
        {
            if (obj is BigInteger i) return i;
            if (obj is bool b) return b ? 1 : 0;
            return 0;
        }
    }
}
