using Shared.Domain;

namespace Client.ViewModels
{
    public class VoyageViewModel : ViewModelBase
    {
        public Voyage Model { get; }

        public VoyageViewModel(Voyage voyage)
        {
            Model = voyage;
        }

        public Ship Ship
        {
            get => Model.Ship;
            set
            {
                Model.Ship = value;
                OnPropertyChanged(nameof(ShipName));
            }
        }

        public Port DeparturePort
        {
            get => Model.DeparturePort;
            set
            {
                Model.DeparturePort = value;
                OnPropertyChanged(nameof(DeparturePortName));
            }
        }

        public Port ArrivalPort
        {
            get => Model.ArrivalPort;
            set
            {
                Model.ArrivalPort = value;
                OnPropertyChanged(nameof(ArrivalPortName));
            }
        }

        public string Code => Model.Code;
        public string Status => Model.Status.ToString();
        public string CaptainMessage => Model.CaptainMessage;
        public double Distance => Model.Distance;
        public DateTime DepartureTime => Model.DepartureTime;
        public DateTime ArrivalTime => Model.ArrivalTime;
        public string ShipName => Model.Ship?.Name ?? "Unknown";
        public string DeparturePortName => Model.DeparturePort?.Name ?? "Unknown";
        public string ArrivalPortName => Model.ArrivalPort?.Name ?? "Unknown";

        public void RefreshAllProperties()
        {
            OnPropertyChanged(nameof(Code));
            OnPropertyChanged(nameof(Status));
            OnPropertyChanged(nameof(CaptainMessage));
            OnPropertyChanged(nameof(Distance));
            OnPropertyChanged(nameof(DepartureTime));
            OnPropertyChanged(nameof(ArrivalTime));
            OnPropertyChanged(nameof(ShipName));
            OnPropertyChanged(nameof(DeparturePortName));
            OnPropertyChanged(nameof(ArrivalPortName));
        }
    }
}