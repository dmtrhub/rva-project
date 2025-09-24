using Shared.Domain;
using Shared.Enums;

namespace Shared.Adapters
{
    // Adapter
    public class CruiseVoyageAdapter : IVoyage
    {
        private readonly Cruise _cruise;

        public CruiseVoyageAdapter(Cruise cruise)
        {
            _cruise = cruise ?? throw new ArgumentNullException(nameof(cruise));
        }

        public string Code => _cruise.Code ?? "CRS-001-002";
        public DateTime ArrivalTime => _cruise.ArrivalTime;
        public DateTime DepartureTime => _cruise.ArrivalTime - _cruise.Duration;
        public VoyageStatus Status => VoyageStatus.Scheduled;

        public string CaptainMessage =>
            $"Cruise '{_cruise.CruiseName}'. {_cruise.StopsNumber} stops, duration: {_cruise.Duration.TotalHours}h";

        public double Distance => _cruise.StopsNumber * 150; // 150km per stop
        public Ship Ship => new Ship { Name = "Cruise Liner" };
        public Port DeparturePort => new Port { Name = "Main Cruise Port", Code = "STR" };
        public Port ArrivalPort => new Port { Name = "Final Destination", Code = "END" };

        public string VoyageSpecs() =>
            $"CRUISE: {_cruise.CruiseName} | Code: {Code} | " +
            $"{DepartureTime:dd/MM/yyyy HH:mm} - {ArrivalTime:dd/MM/yyyy HH:mm} | " +
            $"Stops: {_cruise.StopsNumber} | Distance: {Distance}km";

        // Dodatna svojstva specifična za cruise
        public string CruiseName => _cruise.CruiseName;

        public int StopsNumber => _cruise.StopsNumber;
        public TimeSpan Duration => _cruise.Duration;
        public string Description => _cruise.Description;
    }
}