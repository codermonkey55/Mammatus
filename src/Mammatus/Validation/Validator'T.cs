using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mammatus.Validation
{
    public class Validator<T> : IValidator<T>
    {

        public void AssertValidation(T instance)
        {
            throw new NotImplementedException();
        }

        public bool IsValid(T instance)
        {
            throw new NotImplementedException();
        }

        public ValidationResult Validate(T instance)
        {
            throw new NotImplementedException();
        }
    }
}
