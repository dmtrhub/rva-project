using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

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