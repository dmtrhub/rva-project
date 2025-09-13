using FluentValidation;
using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Validation
{
    public class PortValidator : AbstractValidator<Port>
    {
        public PortValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Port name is required")
                .MaximumLength(100).WithMessage("Port name cannot exceed 100 characters");

            RuleFor(p => p.Country)
                .NotEmpty().WithMessage("Country is required")
                .MaximumLength(50).WithMessage("Country name cannot exceed 50 characters");

            RuleFor(p => p.Code)
                .NotEmpty().WithMessage("Port code is required")
                .Length(3).WithMessage("Port code must be exactly 3 characters")
                .Matches("^[A-Z]{3}$").WithMessage("Port code must contain only uppercase letters");

            RuleFor(p => p.Latitude)
                .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90");

            RuleFor(p => p.Longitude)
                .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180");
        }
    }
}