using Shared.Domain;
using System.ComponentModel;

namespace Client.ViewModels
{
    public class PortViewModel : INotifyPropertyChanged
    {
        public Port Model { get; }

        public PortViewModel(Port model)
        {
            Model = model;
        }

        public string Name
        {
            get => Model.Name;
            set { Model.Name = value; OnPropertyChanged(nameof(Name)); }
        }

        public string Country
        {
            get => Model.Country;
            set { Model.Country = value; OnPropertyChanged(nameof(Country)); }
        }

        public string Code
        {
            get => Model.Code;
            set { Model.Code = value; OnPropertyChanged(nameof(Code)); }
        }

        public double Latitude
        {
            get => Model.Latitude;
            set { Model.Latitude = value; OnPropertyChanged(nameof(Latitude)); }
        }

        public double Longitude
        {
            get => Model.Longitude;
            set { Model.Longitude = value; OnPropertyChanged(nameof(Longitude)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}