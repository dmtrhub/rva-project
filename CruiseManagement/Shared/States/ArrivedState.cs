using Shared.Domain;
using Shared.Enums;

namespace Shared.States
{
    public class ArrivedState : VoyageState
    {
        public override VoyageStatus Status => VoyageStatus.Arrived;

        public override void UpdateState(Voyage voyage)
        {
            voyage.CaptainMessage = GetCaptainMessage();
            voyage.Distance = CalculateDistance(voyage.Distance);
        }

        public override string GetCaptainMessage() =>
            "Voyage has successfully arrived at destination. Thank you for traveling with us.";

        public override double CalculateDistance(double currentDistance) => currentDistance;
    }
}