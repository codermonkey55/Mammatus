using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mammatus.Validation;
using Mammatus.Interface.Contracts;

namespace Mammatus.Interface.Contracts
{
    using System;

    public interface IValidator
    {
        bool CanValidateInstancesOfType(Type type);

        ValidationResult Validate(object instance);

        ValidationResult Validate(ValidationContext context);
    }
}
