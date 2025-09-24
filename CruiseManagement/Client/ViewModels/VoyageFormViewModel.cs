using Shared.Domain;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Client.ViewModels
{
    public class VoyageFormViewModel : INotifyPropertyChanged
    {
        private readonly VoyageViewModel _voyageVm;
        private readonly ObservableCollection<Ship> _availableShips;
        private readonly ObservableCollection<Port> _availablePorts;

        public event Action RequestAddNewShip;

        public event Action RequestAddNewPort;

        public VoyageFormViewModel(VoyageViewModel voyageVm, ObservableCollection<Ship> ships, ObservableCollection<Port> ports)
        {
            _voyageVm = voyageVm;
            _availableShips = ships;
            _availablePorts = ports;

            AddShipCommand = new RelayCommand(_ => RequestAddNewShip?.Invoke());
            AddPortCommand = new RelayCommand(_ => RequestAddNewPort?.Invoke());
        }

        public string Code
        {
            get => _voyageVm.Model.Code;
            set
            {
                _voyageVm.Model.Code = value;
                OnPropertyChanged();
                _voyageVm.RefreshAllProperties();
            }
        }

        public DateTime DepartureTime
        {
            get => _voyageVm.Model.DepartureTime;
            set
            {
                _voyageVm.Model.DepartureTime = value;
                OnPropertyChanged();
                _voyageVm.RefreshAllProperties();
            }
        }

        public DateTime ArrivalTime
        {
            get => _voyageVm.Model.ArrivalTime;
            set
            {
                _voyageVm.Model.ArrivalTime = value;
                OnPropertyChanged();
                _voyageVm.RefreshAllProperties();
            }
        }

        public string CaptainMessage
        {
            get => _voyageVm.Model.CaptainMessage;
            set
            {
                _voyageVm.Model.CaptainMessage = value;
                OnPropertyChanged();
                _voyageVm.RefreshAllProperties();
            }
        }

        public double Distance
        {
            get => _voyageVm.Model.Distance;
            set
            {
                _voyageVm.Model.Distance = value;
                OnPropertyChanged();
                _voyageVm.RefreshAllProperties();
            }
        }

        // ISPRAVKA: Koristi Model.Ship umesto _voyageVm.Ship (koji ne postoji)
        public Ship SelectedShip
        {
            get => _voyageVm.Model.Ship;
            set
            {
                _voyageVm.Model.Ship = value;
                OnPropertyChanged();
                _voyageVm.RefreshAllProperties();
            }
        }

        // ISPRAVKA: Koristi Model.DeparturePort
        public Port SelectedDeparturePort
        {
            get => _voyageVm.Model.DeparturePort;
            set
            {
                _voyageVm.Model.DeparturePort = value;
                OnPropertyChanged();
                _voyageVm.RefreshAllProperties();
            }
        }

        // ISPRAVKA: Koristi Model.ArrivalPort
        public Port SelectedArrivalPort
        {
            get => _voyageVm.Model.ArrivalPort;
            set
            {
                _voyageVm.Model.ArrivalPort = value;
                OnPropertyChanged();
                _voyageVm.RefreshAllProperties();
            }
        }

        // Kolekcije za ComboBox-ove
        public ObservableCollection<Ship> AvailableShips => _availableShips;

        public ObservableCollection<Port> AvailablePorts => _availablePorts;

        // Komande za dugmiće
        public ICommand AddShipCommand { get; }

        public ICommand AddPortCommand { get; }

        public bool IsEditable => _voyageVm.Model != null;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}