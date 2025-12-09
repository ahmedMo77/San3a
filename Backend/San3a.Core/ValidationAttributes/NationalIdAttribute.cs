using System.ComponentModel.DataAnnotations;

namespace San3a.Core.ValidationAttributes
{
    public class NationalIdAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success; // Allow null or empty (use [Required] separately if needed)
            }

            var nationalId = value.ToString();

            // Check if exactly 14 digits
            if (nationalId.Length != 14)
            {
                return new ValidationResult("National ID must be exactly 14 digits");
            }

            // Check if all characters are digits
            if (!nationalId.All(char.IsDigit))
            {
                return new ValidationResult("National ID must contain only digits");
            }

            return ValidationResult.Success;
        }
    }
}
