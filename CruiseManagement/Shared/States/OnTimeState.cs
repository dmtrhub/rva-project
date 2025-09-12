using Shared.Domain;
using Shared.Enums;

namespace Shared.States
{
    public class OnTimeState : VoyageState
    {
        public override VoyageStatus Status => VoyageStatus.OnTime;

        public override void UpdateState(Voyage voyage)
        {
            voyage.CaptainMessage = GetCaptainMessage();
            voyage.Distance = CalculateDistance(voyage.Distance);
        }

        public override string GetCaptainMessage() =>
            "Voyage is on time. All systems operational and proceeding as planned.";

        public override double CalculateDistance(double currentDistance) =>
            currentDistance + new Random().Next(50, 200);
    }
}