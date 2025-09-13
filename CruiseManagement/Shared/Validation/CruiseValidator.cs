using FluentValidation;
using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Validation
{
    public class CruiseValidator : AbstractValidator<Cruise>
    {
        public CruiseValidator()
        {
            RuleFor(c => c.Code)
                .NotEmpty().WithMessage("Cruise code is required")
                .Matches(@"^[A-Z]{3}-\d{3}-\d{3}$")
                .WithMessage("Cruise code must be in format: ABC-123-456");

            RuleFor(c => c.ArrivalTime)
                .NotEmpty().WithMessage("Arrival time is required")
                .GreaterThan(DateTime.Now).WithMessage("Arrival time must be in the future");

            RuleFor(c => c.Duration)
                .GreaterThan(TimeSpan.Zero).WithMessage("Duration must be greater than 0");

            RuleFor(c => c.StopsNumber)
                .GreaterThanOrEqualTo(0).WithMessage("Stops number cannot be negative");
        }
    }
}