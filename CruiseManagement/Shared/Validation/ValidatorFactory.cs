using FluentValidation;
using Shared.Domain;

namespace Shared.Validation
{
    public class ValidatorFactory : IValidatorFactory
    {
        public IValidator<Voyage> GetVoyageValidator() => new VoyageValidator();

        public IValidator<Ship> GetShipValidator() => new ShipValidator();

        public IValidator<Port> GetPortValidator() => new PortValidator();

        public IValidator<Cruise> GetCruiseValidator() => new CruiseValidator();
    }
}