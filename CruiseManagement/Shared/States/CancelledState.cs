using Shared.Domain;
using Shared.Enums;

namespace Shared.States
{
    public class CancelledState : VoyageState
    {
        public override VoyageStatus Status => VoyageStatus.Cancelled;

        public override void UpdateState(Voyage voyage)
        {
            voyage.CaptainMessage = GetCaptainMessage();
            voyage.Distance = CalculateDistance(voyage.Distance);
        }

        public override string GetCaptainMessage() =>
            "Voyage has been cancelled. All passengers will be notified of alternative arrangements.";

        public override double CalculateDistance(double currentDistance) => currentDistance;
    }
}