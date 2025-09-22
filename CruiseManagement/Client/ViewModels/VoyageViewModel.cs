using Shared.Domain;
using Shared.Enums;
using System.ComponentModel;

namespace Client.ViewModels
{
    public class VoyageViewModel : INotifyPropertyChanged
    {
        public Voyage Model { get; }

        public VoyageViewModel(Voyage model)
        {
            Model = model;
        }

        public string Code
        {
            get => Model.Code;
            set { Model.Code = value; OnPropertyChanged(nameof(Code)); }
        }

        public DateTime ArrivalTime
        {
            get => Model.ArrivalTime;
            set { Model.ArrivalTime = value; OnPropertyChanged(nameof(ArrivalTime)); }
        }

        public DateTime DepartureTime
        {
            get => Model.DepartureTime;
            set { Model.DepartureTime = value; OnPropertyChanged(nameof(DepartureTime)); }
        }

        public string CaptainMessage
        {
            get => Model.CaptainMessage;
            set { Model.CaptainMessage = value; OnPropertyChanged(nameof(CaptainMessage)); }
        }

        public double Distance
        {
            get => Model.Distance;
            set { Model.Distance = value; OnPropertyChanged(nameof(Distance)); }
        }

        public string Status
        {
            get => Model.Status.ToString();
            set
            {
                if (Enum.TryParse<VoyageStatus>(value, out var status))
                {
                    Model.Status = status;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        public Ship Ship
        {
            get => Model.Ship;
            set { Model.Ship = value; OnPropertyChanged(nameof(Ship)); }
        }

        public Port DeparturePort
        {
            get => Model.DeparturePort;
            set { Model.DeparturePort = value; OnPropertyChanged(nameof(DeparturePort)); }
        }

        public Port ArrivalPort
        {
            get => Model.ArrivalPort;
            set { Model.ArrivalPort = value; OnPropertyChanged(nameof(ArrivalPort)); }
        }

        public string ShipName => Ship?.Name ?? "No ship assigned";
        public string DeparturePortName => DeparturePort?.Name ?? "No departure port";
        public string ArrivalPortName => ArrivalPort?.Name ?? "No arrival port";

        public event PropertyChangedEventHandler PropertyChanged;

        public void UpdateFrom(Voyage source)
        {
            Code = source.Code;
            DepartureTime = source.DepartureTime;
            ArrivalTime = source.ArrivalTime;
            CaptainMessage = source.CaptainMessage;
            Distance = source.Distance;
            Ship = source.Ship;
            DeparturePort = source.DeparturePort;
            ArrivalPort = source.ArrivalPort;
            OnPropertyChanged(nameof(Status));
        }

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}