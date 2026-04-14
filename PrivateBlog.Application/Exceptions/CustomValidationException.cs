using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace PrivateBlog.Application.Exceptions
{
    public class CustomValidationException : Exception
    {
        public List<string> Errors { get; set; } = [];

        public CustomValidationException(ValidationResult validationResult)
        {
            Errors.AddRange(validationResult.Errors.Select(error => error.ErrorMessage));
        }

        public CustomValidationException(string errorMessage)
        {
            Errors.Add(errorMessage);
        }
    }
}
