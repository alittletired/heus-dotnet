namespace Heus.AspNetCore.Validation;

public class ValidationException:Exception
{
    public ValidationResult ValidationResult { get; }
    public ValidationException(string message, ValidationResult validationResult)
        : base(message)
    {
        ValidationResult = validationResult;
    }
}