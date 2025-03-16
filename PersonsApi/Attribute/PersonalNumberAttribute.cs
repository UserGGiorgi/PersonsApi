using PersonsApi.Data;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PersonsApi.Attribute
{
    public class PersonalNumberAttribute : ValidationAttribute
    {
        private const int RequiredLength = 11;
        private static readonly Regex PersonalNumberRegex = new(@"^\d{11}$", RegexOptions.Compiled);

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string personalNumber)
            {
                if (personalNumber.Length != RequiredLength)
                {
                    return new ValidationResult($"Personal number must be exactly {RequiredLength} digits.");
                }

                if (!PersonalNumberRegex.IsMatch(personalNumber))
                {
                    return new ValidationResult("Personal number must contain only digits.");
                }

                var dbContext = validationContext.GetRequiredService<AppDbContext>();
                var isUnique = !dbContext.People.Any(p => p.PersonalNumber == personalNumber);

                if (!isUnique)
                {
                    return new ValidationResult("Personal number must be unique.");
                }

                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid personal number format.");
        }
    }
}
