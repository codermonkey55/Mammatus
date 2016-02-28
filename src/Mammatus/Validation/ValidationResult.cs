using System.Collections;
using System.Collections.Generic;

namespace Mammatus.Validation
{
    public sealed class ValidationResult : IEnumerable<ValidationError>
    {
        private readonly List<ValidationError> _errors = new List<ValidationError>();

        public ValidationResult()
        {
        }

        public ValidationResult(IEnumerable<ValidationError> errors)
        : this()
        {
            this._errors.AddRange(errors);
        }

        public IEnumerable<ValidationError> Errors
        {
            get
            {
                foreach (ValidationError error in this._errors)
                {
                    yield return error;
                }
            }
        }

        public bool IsValid
        {
            get
            {
                return this._errors.Count == 0;
            }
        }

        public void AddError(ValidationError error)
        {
            this._errors.Add(error);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Errors.GetEnumerator();
        }

        IEnumerator<ValidationError> IEnumerable<ValidationError>.GetEnumerator()
        {
            return this.Errors.GetEnumerator();
        }

        public void RemoveError(ValidationError error)
        {
            if (this._errors.Contains(error))
            {
                this._errors.Remove(error);
            }
        }
    }
}