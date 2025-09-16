using Shared.Domain;
using Shared.Enums;
using System.Collections;
using System.ComponentModel;

namespace Client.ViewModels
{
    public class ShipViewModel : INotifyPropertyChanged
    {
        public Ship Model { get; }

        public ShipViewModel(Ship model)
        {
            Model = model;
        }

        public string Name
        {
            get => Model.Name;
            set { Model.Name = value; OnPropertyChanged(nameof(Name)); }
        }

        public int Capacity
        {
            get => Model.Capacity;
            set { Model.Capacity = value; OnPropertyChanged(nameof(Capacity)); }
        }

        public double LengthInMeters
        {
            get => Model.LengthInMeters;
            set { Model.LengthInMeters = value; OnPropertyChanged(nameof(LengthInMeters)); }
        }

        public ShipType Type
        {
            get => Model.Type;
            set { Model.Type = value; OnPropertyChanged(nameof(Type)); }
        }

        public double DraftInMeters
        {
            get => Model.DraftInMeters;
            set { Model.DraftInMeters = value; OnPropertyChanged(nameof(DraftInMeters)); }
        }

        public IEnumerable ShipTypeValues
        {
            get { return Enum.GetValues(typeof(ShipType)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}