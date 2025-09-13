using FluentValidation;
using Shared.Domain;

namespace Shared.Validation
{
    public interface IValidatorFactory
    {
        IValidator<Voyage> GetVoyageValidator();

        IValidator<Ship> GetShipValidator();

        IValidator<Port> GetPortValidator();

        IValidator<Cruise> GetCruiseValidator();
    }
}