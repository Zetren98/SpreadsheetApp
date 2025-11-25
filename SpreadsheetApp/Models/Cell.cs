using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SpreadsheetApp.Models
{
    public class Cell : INotifyPropertyChanged
    {
        public string Address { get; }
        public string Expression { get; set; } = "";

        private string _value;
        public string Value
        {
            get => _value;
            set { _value = value; OnPropertyChanged(); }
        }

        public Cell(string address)
        {
            Address = address;
            Value = "";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}