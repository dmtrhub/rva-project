using FluentValidation;
using Shared.Domain;

namespace Shared.Validation
{
    public class ShipValidator : AbstractValidator<Ship>
    {
        public ShipValidator()
        {
            RuleFor(s => s.Name)
                .NotEmpty().WithMessage("Ship name is required")
                .MaximumLength(100).WithMessage("Ship name cannot exceed 100 characters");

            RuleFor(s => s.Capacity)
                .GreaterThan(0).WithMessage("Capacity must be greater than 0");

            RuleFor(s => s.LengthInMeters)
                .GreaterThan(0).WithMessage("Length must be greater than 0");

            RuleFor(s => s.DraftInMeters)
                .GreaterThan(0).WithMessage("Draft must be greater than 0");

            RuleFor(s => s.Type)
                .IsInEnum().WithMessage("Invalid ship type");
        }
    }
}