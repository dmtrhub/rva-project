using Shared.Domain;
using Shared.Enums;

namespace Shared.States
{
    public class DelayedState : VoyageState
    {
        public override VoyageStatus Status => VoyageStatus.Delayed;

        public override void UpdateState(Voyage voyage)
        {
            voyage.CaptainMessage = GetCaptainMessage();
            voyage.Distance = CalculateDistance(voyage.Distance);
        }

        public override string GetCaptainMessage() =>
            "Voyage is delayed due to weather conditions. Expected to resume shortly.";

        public override double CalculateDistance(double currentDistance) =>
            currentDistance + new Random().Next(10, 50);
    }
}