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
            _cruise = cruise;
        }

        public string Code => _cruise.Code;
        public DateTime ArrivalTime => _cruise.ArrivalTime;
        public DateTime DepartureTime => _cruise.ArrivalTime - _cruise.Duration;
        public VoyageStatus Status => VoyageStatus.Scheduled; // Default status for cruises

        public string CaptainMessage =>
            $"Cruise with {_cruise.StopsNumber} stops. Duration: {_cruise.Duration.TotalHours} hours.";

        public double Distance => _cruise.StopsNumber * 100; // 100km per stop

        public string VoyageSpecs() =>
            $"Cruise {Code} | {DepartureTime} - {ArrivalTime} | Stops: {_cruise.StopsNumber} | Distance: {Distance}km";
    }
}