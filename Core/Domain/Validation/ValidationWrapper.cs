using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Core.Domain.Validation;

/// <summary>
/// Validation Wrapper to be used for model validation in Azure Functions
/// Found at https://medium.com/javarevisited/model-validation-for-http-triggered-azure-functions-in-c-130676c2d490
/// </summary>
public class ValidationWrapper<T>
{
    public bool IsValid { get; set; }
    public T Value { get; set; }
    public IEnumerable<ValidationResult> ValidationResults { get; set; }

    public static ValidationWrapper<T> BuildValidationWrapper(string bodyString)
    {
        ValidationWrapper<T> body = new ValidationWrapper<T>();

        body.Value = JsonConvert.DeserializeObject<T>(bodyString);

        var results = new List<ValidationResult>();
        body.IsValid = Validator.TryValidateObject(body.Value, new ValidationContext(body.Value, null, null), results, true);
        body.ValidationResults = results;

        return body;
    }
}
