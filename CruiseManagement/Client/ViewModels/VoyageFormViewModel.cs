using Shared.Domain;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Client.ViewModels
{
    public class VoyageFormViewModel : INotifyPropertyChanged
    {
        private readonly VoyageViewModel _voyage;
        private readonly ObservableCollection<Ship> _availableShips;
        private readonly ObservableCollection<Port> _availablePorts;

        public event Action RequestAddNewShip;

        public event Action RequestAddNewPort;

        public VoyageFormViewModel(VoyageViewModel voyage, ObservableCollection<Ship> ships, ObservableCollection<Port> ports)
        {
            _voyage = voyage;
            _availableShips = ships;
            _availablePorts = ports;

            AddShipCommand = new RelayCommand(_ => RequestAddNewShip?.Invoke());
            AddPortCommand = new RelayCommand(_ => RequestAddNewPort?.Invoke());
        }

        public string Code
        {
            get => _voyage.Code;
            set { _voyage.Code = value; OnPropertyChanged(); }
        }

        public DateTime DepartureTime
        {
            get => _voyage.DepartureTime;
            set { _voyage.DepartureTime = value; OnPropertyChanged(); }
        }

        public DateTime ArrivalTime
        {
            get => _voyage.ArrivalTime;
            set { _voyage.ArrivalTime = value; OnPropertyChanged(); }
        }

        public string CaptainMessage
        {
            get => _voyage.CaptainMessage;
            set { _voyage.CaptainMessage = value; OnPropertyChanged(); }
        }

        public double Distance
        {
            get => _voyage.Distance;
            set { _voyage.Distance = value; OnPropertyChanged(); }
        }

        public Ship SelectedShip
        {
            get => _voyage.Ship;
            set { _voyage.Ship = value; OnPropertyChanged(); }
        }

        public Port SelectedDeparturePort
        {
            get => _voyage.DeparturePort;
            set { _voyage.DeparturePort = value; OnPropertyChanged(); }
        }

        public Port SelectedArrivalPort
        {
            get => _voyage.ArrivalPort;
            set { _voyage.ArrivalPort = value; OnPropertyChanged(); }
        }

        // Kolekcije za ComboBox-ove
        public ObservableCollection<Ship> AvailableShips => _availableShips;

        public ObservableCollection<Port> AvailablePorts => _availablePorts;

        // Komande za dugmiće
        public ICommand AddShipCommand { get; }

        public ICommand AddPortCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}