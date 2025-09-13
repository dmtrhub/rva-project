using FluentValidation;
using Shared.Validation;

namespace Shared.Domain
{
    public abstract class BaseEntity
    {
        public virtual bool Validate()
        {
            return GetValidationErrors().Count == 0;
        }

        public virtual List<string> GetValidationErrors()
        {
            // Osnovna validacija - override u izvedenim klasama
            return new List<string>();
        }

        // Generic metoda za FluentValidation
        protected List<string> ValidateWithFluent<TEntity>(IValidator<TEntity> validator)
            where TEntity : BaseEntity
        {
            if (validator == null)
                return new List<string>();

            var result = validator.Validate(this as TEntity);
            return result.Errors.Select(e => e.ErrorMessage).ToList();
        }
    }
}