using SpreadsheetApp.Models;
using Cell = SpreadsheetApp.Models.Cell;

namespace SpreadsheetApp.Services
{
    public class Spreadsheet
    {
        private readonly int _rowCount;
        private readonly int _colCount;
        public Dictionary<string, Cell> Cells { get; private set; } = new();

        public Spreadsheet(int rows, int cols)
        {
            _rowCount = rows;
            _colCount = cols;
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            Cells.Clear();
            for (int r = 1; r <= _rowCount; r++)
            {
                for (int c = 0; c < _colCount; c++)
                {
                    string address = GetColumnName(c) + r;
                    Cells.Add(address, new Cell(address));
                }
            }
        }

        public void RecalculateAll()
        {
            foreach (var cell in Cells.Values)
            {
                if (string.IsNullOrWhiteSpace(cell.Expression))
                {
                    cell.Value = "";
                    continue;
                }

                if (!cell.Expression.StartsWith("="))
                {
                    cell.Value = cell.Expression;
                    continue;
                }

                string formula = cell.Expression.Substring(1);

                cell.Value = AntlrParser.Evaluate(formula, Cells);
            }
        }

        public static string GetColumnName(int index)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string value = "";
            if (index >= letters.Length) value += letters[index / letters.Length - 1];
            value += letters[index % letters.Length];
            return value;
        }
    }
}