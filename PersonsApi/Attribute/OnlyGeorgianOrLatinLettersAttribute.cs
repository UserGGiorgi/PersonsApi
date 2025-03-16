using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PersonsApi.Attribute
{
    public class OnlyGeorgianOrLatinLettersAttribute : ValidationAttribute
    {
        private static readonly Regex GeorgianRegex = new Regex(@"^[\u10A0-\u10FF]+$");
        private static readonly Regex LatinRegex = new Regex(@"^[a-zA-Z]+$");

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success;
            }
            string input = value?.ToString() ?? string.Empty;
            if (input == null)
            {
                return new ValidationResult("Property Is Null");
            }

            bool isGeorgian = GeorgianRegex.IsMatch(input);
            bool isLatin = LatinRegex.IsMatch(input);

            if (!isGeorgian && !isLatin)
            {
                return new ValidationResult("The field must contain only Georgian or Latin letters.");
            }

            if (isGeorgian && isLatin)
            {
                return new ValidationResult("The field must not contain both Georgian and Latin letters.");
            }

            return ValidationResult.Success;
        }
    }
}
