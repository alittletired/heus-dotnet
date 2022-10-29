namespace Heus.AspNetCore.Validation;

public class ValidationResult
{
    public List<Tuple<string, IEnumerable<string>>> Errors { get; } = new();
    public void AddError(string message , IEnumerable<string> memberNames)
    {
        Errors.Add(new Tuple<string, IEnumerable<string>>(message,memberNames));
       
    }
}