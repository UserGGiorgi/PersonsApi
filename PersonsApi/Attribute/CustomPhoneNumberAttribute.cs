using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PersonsApi.Attribute
{
    public class CustomPhoneNumberAttribute : ValidationAttribute
    {
        private const int MinLength = 4;
        private const int MaxLength = 50;
        private static readonly Regex NumericRegex = new Regex(@"^\+?\d+$", RegexOptions.Compiled);

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string strValue)
            {
                if (strValue.Length < MinLength || strValue.Length > MaxLength)
                {
                    return new ValidationResult($"The field must be between {MinLength} and {MaxLength} characters long.");
                }

                if (!NumericRegex.IsMatch(strValue))
                {
                    return new ValidationResult("The field must contain only digits and may start with a '+' sign.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
