using FluentValidation;
using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Validation
{
    public class VoyageValidator : AbstractValidator<Voyage>
    {
        public VoyageValidator()
        {
            RuleFor(v => v.Code)
                .NotEmpty().WithMessage("Voyage code is required")
                .Matches(@"^[A-Z]{3}-\d{3}-\d{3}$")
                .WithMessage("Voyage code must be in format: ABC-123-456");

            RuleFor(v => v.DepartureTime)
                .NotEmpty().WithMessage("Departure time is required")
                .LessThan(v => v.ArrivalTime)
                .WithMessage("Departure time must be before arrival time");

            RuleFor(v => v.ArrivalTime)
                .NotEmpty().WithMessage("Arrival time is required")
                .GreaterThan(v => v.DepartureTime)
                .WithMessage("Arrival time must be after departure time");

            RuleFor(v => v.Distance)
                .GreaterThanOrEqualTo(0).WithMessage("Distance cannot be negative");

            RuleFor(v => v.CaptainMessage)
                .MaximumLength(500).WithMessage("Captain message cannot exceed 500 characters");
        }
    }
}