using Shared.Domain;
using Shared.Enums;

namespace Shared.States
{
    public class ScheduledState : VoyageState
    {
        public override VoyageStatus Status => VoyageStatus.Scheduled;

        public override void UpdateState(Voyage voyage)
        {
            voyage.CaptainMessage = GetCaptainMessage();
            voyage.Distance = CalculateDistance(voyage.Distance);
        }

        public override string GetCaptainMessage() =>
            "Voyage is scheduled. All systems are being prepared for departure.";

        public override double CalculateDistance(double currentDistance) => 0;
    }
}