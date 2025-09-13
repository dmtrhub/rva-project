using FluentValidation;
using Shared.Adapters;
using Shared.Enums;
using Shared.States;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Shared.Domain
{
    public class Voyage : BaseEntity, INotifyPropertyChanged, IVoyage
    {
        private string _code;
        private DateTime _arrivalTime;
        private DateTime _departureTime;
        private string _captainMessage;
        private double _distance;
        private VoyageState _currentState;
        private VoyageStatus _status;

        public event PropertyChangedEventHandler PropertyChanged;

        public Voyage()
        {
            // Početno stanje
            TransitionTo(new ScheduledState());
        }

        public string Code { get => _code; set => SetField(ref _code, value); }
        public DateTime ArrivalTime { get => _arrivalTime; set => SetField(ref _arrivalTime, value); }
        public DateTime DepartureTime { get => _departureTime; set => SetField(ref _departureTime, value); }
        public string CaptainMessage { get => _captainMessage; set => SetField(ref _captainMessage, value); }
        public double Distance { get => _distance; set => SetField(ref _distance, value); }
        public VoyageStatus Status { get => _status; private set => SetField(ref _status, value); }

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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}