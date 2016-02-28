using System;
using System.Collections.Generic;

using Mammatus.Validation;

namespace Mammatus.Domain.Core
{
    [Serializable]
    public abstract class ValidatableObject<TEntity> : IValidatable<TEntity>
        where TEntity : ValidatableObject<TEntity>
    {
        private IValidator<TEntity> validator;

        protected IValidator<TEntity> Validator
        {
            get
            {
                if (this.validator == null)
                {
                    this.validator = new Validator<TEntity>();
                }

                return this.validator;
            }
        }

        public virtual void AssertValidation()
        {
            ValidationResult result = this.Validate();

            if (!result.IsValid)
            {
                throw new ValidationException(this.GetType(), result.Errors);
            }
        }

        public virtual bool IsValid()
        {
            return this.Validate().IsValid;
        }

        public virtual ValidationResult Validate()
        {
            return this.Validator.Validate((TEntity)this);
        }

        public virtual ValidationResult Validate(IValidator<TEntity> validator)
        {
            return validator.Validate((TEntity)this);
        }
    }
}