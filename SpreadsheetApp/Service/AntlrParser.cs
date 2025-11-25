using Antlr4.Runtime;
using SpreadsheetApp.Models;
using SpreadsheetApp.Service;
using Cell = SpreadsheetApp.Models.Cell;

namespace SpreadsheetApp.Services
{
    public static class AntlrParser
    {
        public static string Evaluate(string expression, Dictionary<string, Cell> cells)
        {
            try
            {
                var inputStream = new AntlrInputStream(expression.ToLower());

                var lexer = new LabCalculatorLexer(inputStream);

                var tokenStream = new CommonTokenStream(lexer);

                var parser = new LabCalculatorParser(tokenStream);

                parser.RemoveErrorListeners();
 
                parser.AddErrorListener(new ThrowExceptionErrorListener());

                var tree = parser.compileUnit();

                var visitor = new CalculatorVisitor(cells);
                return visitor.Visit(tree).ToString();
            }
            catch (Exception ex)
            {
                return $"Err: {ex.Message}";
            }
        }
    }
}