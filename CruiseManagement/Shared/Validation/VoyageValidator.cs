using FluentValidation;
using Shared.Domain;

namespace Shared.Validation
{
    public class VoyageValidator : AbstractValidator<Voyage>
    {
        private readonly List<Voyage> _existingVoyages;

        public VoyageValidator(List<Voyage> existingVoyages = null)
        {
            RuleFor(v => v.Code)
                .NotEmpty().WithMessage("Voyage code is required")
                .Matches(@"^[A-Z]{3}-\d{3}-\d{3}$")
                .WithMessage("Voyage code must be in format: ABC-123-456")
                .Must(BeUniqueCode).WithMessage("Voyage code must be unique");

            RuleFor(v => v.DepartureTime)
                .NotEmpty().WithMessage("Departure time is required")
                .LessThan(v => v.ArrivalTime)
                .WithMessage("Departure time must be before arrival time");

            RuleFor(v => v.ArrivalTime)
                .NotEmpty().WithMessage("Arrival time is required")
                .GreaterThan(v => v.DepartureTime)
                .WithMessage("Arrival time must be after departure time");

            RuleFor(v => v.Distance)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Distance cannot be negative");

            RuleFor(v => v.CaptainMessage)
                .MaximumLength(500)
                .WithMessage("Captain message cannot exceed 500 characters");

            RuleFor(v => v.Ship)
                .NotNull()
                .WithMessage("Ship is required");

            RuleFor(v => v.DeparturePort)
                .NotNull()
                .WithMessage("Departure port is required");

            RuleFor(v => v.ArrivalPort)
                .NotNull()
                .WithMessage("Arrival port is required");

            RuleFor(v => v)
            .Must(v => v.DeparturePort == null ||
                       v.ArrivalPort == null ||
                       v.DeparturePort.Code != v.ArrivalPort.Code)
            .WithMessage("Departure and arrival ports cannot be the same")
            .OverridePropertyName("DeparturePort");
        }

        private bool BeUniqueCode(string code)
        {
            if (_existingVoyages == null) return true;

            return !_existingVoyages.Any(v => v.Code.Equals(code, System.StringComparison.OrdinalIgnoreCase));
        }
    }
}