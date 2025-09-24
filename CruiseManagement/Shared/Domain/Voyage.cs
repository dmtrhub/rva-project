using Shared.Adapters;
using Shared.Enums;
using Shared.States;

namespace Shared.Domain
{
    public class Voyage : BaseEntity, IVoyage
    {
        public Voyage()
        {
            // Početno stanje
            TransitionTo(new ScheduledState());
        }

        public string Code { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime DepartureTime { get; set; }
        public string CaptainMessage { get; set; }
        public double Distance { get; set; }
        public VoyageStatus Status { get; set; }

        // Add these properties:
        public Ship Ship { get; set; }

        public Port DeparturePort { get; set; }
        public Port ArrivalPort { get; set; }

        private VoyageState _currentState;

        public void TransitionTo(VoyageState state)
        {
            _currentState = state;
            Status = state.Status;
            _currentState.UpdateState(this);
        }

        public void SimulateStateChange()
        {
            // Logika za promenu stanja po zadatom redosledu
            if (_currentState is ScheduledState)
                TransitionTo(new OnTimeState());
            else if (_currentState is OnTimeState)
                TransitionTo(new DelayedState());
            else if (_currentState is DelayedState)
                TransitionTo(new ArrivedState());
            else if (_currentState is ArrivedState)
                TransitionTo(new CancelledState());
            else if (_currentState is CancelledState)
                TransitionTo(new ScheduledState());
        }

        public string VoyageSpecs() =>
            $"Voyage {Code} | {DepartureTime} - {ArrivalTime} | Distance: {Distance} km | Status: {Status}";

        public Voyage Clone()
        {
            return new Voyage
            {
                Code = this.Code,
                ArrivalTime = this.ArrivalTime,
                DepartureTime = this.DepartureTime,
                CaptainMessage = this.CaptainMessage,
                Distance = this.Distance,
                Status = this.Status
            };
        }
    }
}