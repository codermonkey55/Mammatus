namespace Mammatus.Validation
{
    public interface IValidator<TEntity>
    {
        void AssertValidation(TEntity instance);

        bool IsValid(TEntity instance);

        ValidationResult Validate(TEntity instance);
    }
}