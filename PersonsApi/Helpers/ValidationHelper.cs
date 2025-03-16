using System.ComponentModel.DataAnnotations;

namespace PersonsApi.Helpers
{
    public static class ValidationHelper
    {
        public static ValidationResult ValidateDateOfBirth(DateTime dateOfBirth)
        {
            var age = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth > DateTime.Today.AddYears(-age)) age--;

            if (age < 18)
                return new ValidationResult("Person must be at least 18 years old.");

            return ValidationResult.Success;
        }
    }
}
