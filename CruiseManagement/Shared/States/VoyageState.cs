using Shared.Domain;
using Shared.Enums;

namespace Shared.States
{
    public abstract class VoyageState
    {
        public abstract VoyageStatus Status { get; }

        public abstract void UpdateState(Voyage voyage);

        public abstract string GetCaptainMessage();

        public abstract double CalculateDistance(double currentDistance);

        public virtual bool CanTransitionTo(VoyageState newState) => true;
    }
}