using System.Collections.ObjectModel;
using System.Windows.Input;
using SpreadsheetApp.Services;
using SpreadsheetApp.Models;
using Cell = SpreadsheetApp.Models.Cell;

namespace SpreadsheetApp.ViewModels
{
    public class MainViewModel
    {
        // відображення
        public ObservableCollection<Cell> CellsDisplay { get; set; } = new();

        // кількість колонок
        public int ColumnsCount { get; } = 10;

        // таблиця
        private Spreadsheet _spreadsheet;

        // кнопки
        public ICommand CalculateCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand HelpCommand { get; }

        public MainViewModel()
        {
            _spreadsheet = new Spreadsheet(10, ColumnsCount);
            foreach (var cell in _spreadsheet.Cells.Values)
            {
                CellsDisplay.Add(cell);
            }

            CalculateCommand = new Command(() =>
            {
                _spreadsheet.RecalculateAll();
            });

            SaveCommand = new Command(async () =>
            {
                if (Application.Current?.MainPage != null)
                    await Application.Current.MainPage.DisplayAlert("Info", "Збереження...", "OK");
            });

            HelpCommand = new Command(async () =>
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Довідка",
                        "Лабораторна робота.\nВаріант 69.\n" +
                        "Операції: not, and, or, eqv, =, <, >, +, -.", "OK");
                }
            });
        }
    }
}