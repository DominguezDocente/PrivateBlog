using FluentValidation.Results;

namespace PrivateBlog.Application.Exceptions
{
    public class CustomValidationException : Exception
    {
        public List<string> ValidationErrors { get; set; } = [];

        public CustomValidationException(ValidationResult validationResult)
        {
            ValidationErrors.AddRange(validationResult.Errors.Select(error => error.ErrorMessage));
        }

        public CustomValidationException(string errorMessage)
        {
            ValidationErrors.Add(errorMessage);
        }
    }
}
