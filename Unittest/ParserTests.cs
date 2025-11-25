using Xunit;
using SpreadsheetApp.Services;
using SpreadsheetApp.Models;
using System.Collections.Generic;
using Cell = SpreadsheetApp.Models.Cell;

namespace SpreadsheetApp.Tests
{
    public class ParserTests
    {
        private Dictionary<string, Cell> _mockCells;

        public ParserTests()
        {
            _mockCells = new Dictionary<string, Cell>();
        }

        [Theory]
        [InlineData("5", "5")]
        [InlineData("-5", "-5")]
        [InlineData("+10", "10")]
        [InlineData("5+5", "10")]
        [InlineData("10-2", "8")]
        [InlineData("-(-5)", "5")]
        public void Evaluate_Arithmetic_ReturnsCorrectResult(string expr, string expected)
        {
            var result = AntlrParser.Evaluate(expr, _mockCells);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("true", "True")]
        [InlineData("false", "False")]
        [InlineData("not true", "False")]
        [InlineData("not false", "True")]
        public void Evaluate_LogicUnary_ReturnsCorrectResult(string expr, string expected)
        {
            var result = AntlrParser.Evaluate(expr, _mockCells);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("true eqv true", "True")]
        [InlineData("true eqv false", "False")]
        [InlineData("1 eqv 1", "True")]
        [InlineData("5 eqv 0", "False")]
        public void Evaluate_Eqv_ReturnsCorrectResult(string expr, string expected)
        {
            var result = AntlrParser.Evaluate(expr, _mockCells);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("5 > 2", "True")]
        [InlineData("2 < 5", "True")]
        [InlineData("5 = 5", "True")]
        [InlineData("5 = 6", "False")]
        public void Evaluate_Comparison_ReturnsCorrectResult(string expr, string expected)
        {
            var result = AntlrParser.Evaluate(expr, _mockCells);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Evaluate_CellReference_ReturnsValue()
        {
            var cellA1 = new Cell("A1") { Value = "100" };
            _mockCells.Add("A1", cellA1);
            var result = AntlrParser.Evaluate("A1", _mockCells);
            Assert.Equal("100", result);
        }

        [Fact]
        public void Evaluate_BigInteger_HandlesLargeNumbers()
        {
            string bigNum = "999999999999999999999";
            var result = AntlrParser.Evaluate(bigNum, _mockCells);
            Assert.Equal(bigNum, result);
        }
    }
}
