using PersonsApi.Attribute;
using PersonsApi.Dtos;
using PersonsApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace PersonsApi.DTOs
{
    public class PersonRequestDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        [OnlyGeorgianOrLatinLetters(ErrorMessage = "First name must contain only Georgian or Latin letters, but not both.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [OnlyGeorgianOrLatinLetters(ErrorMessage = "First name must contain only Georgian or Latin letters, but not both.")]
        public string LastName { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [PersonalNumber(ErrorMessage = "Personal number must be exactly 11 digits.")]
        public string PersonalNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(PersonDTO), "ValidateDateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public int CityId { get; set; }

        public List<PhoneNumberDTO> PhoneNumbers { get; set; } = new List<PhoneNumberDTO>();

        public static ValidationResult ValidateDateOfBirth(DateTime dateOfBirth, ValidationContext context)
        {
            var age = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth > DateTime.Today.AddYears(-age)) age--;

            if (age < 18)
                return new ValidationResult("Person must be at least 18 years old.");
            return ValidationResult.Success;
        }
    }
}
