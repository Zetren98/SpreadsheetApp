namespace SpreadsheetApp // <--- МАЄ СПІВПАДАТИ з x:Class="SpreadsheetApp.App"
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Цей рядок створює головне вікно
            MainPage = new AppShell();
            // Якщо у тебе немає AppShell, заміни на: MainPage = new MainPage();
        }
    }
}