using System.Collections.Generic;

namespace Mammatus.Validation
{
    public interface IValidatable
    {
        void AssertValidation();

        bool IsValid();

        ValidationResult Validate();
    }

    public interface IValidatable<T> : IValidatable
    {
        ValidationResult Validate(IValidator<T> validator);
    }
}