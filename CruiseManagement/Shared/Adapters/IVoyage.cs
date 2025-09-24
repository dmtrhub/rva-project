using Shared.Enums;

namespace Shared.Adapters
{
    // Target interface
    public interface IVoyage
    {
        string Code { get; }
        DateTime ArrivalTime { get; }
        DateTime DepartureTime { get; }
        string CaptainMessage { get; }
        double Distance { get; }
        VoyageStatus Status { get; }

        string VoyageSpecs();
    }
}