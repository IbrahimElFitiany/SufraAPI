using System.ComponentModel.DataAnnotations;

namespace Sufra.Validation
{
    public class DateNotInPastAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is DateTime dateTimeValue)
            {
                if (dateTimeValue >= DateTime.UtcNow)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} must be current or future date/time.");
                }
            }

            return new ValidationResult($"{validationContext.DisplayName} is not a valid date.");
        }
    }
}
