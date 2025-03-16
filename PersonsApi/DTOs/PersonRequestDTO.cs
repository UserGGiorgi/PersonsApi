using PersonsApi.Attribute;
using PersonsApi.Dtos;
using PersonsApi.Enums;
using PersonsApi.Helpers;
using System.ComponentModel.DataAnnotations;

namespace PersonsApi.DTOs
{
    public class PersonRequestDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        [OnlyGeorgianOrLatinLetters(ErrorMessage = "First name must contain only Georgian or Latin letters, but not both.")]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [OnlyGeorgianOrLatinLetters(ErrorMessage = "First name must contain only Georgian or Latin letters, but not both.")]
        public string LastName { get; set; } = null!;

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [PersonalNumber(ErrorMessage = "Personal number must be exactly 11 digits.")]
        public string PersonalNumber { get; set; } = null!;

        [Required]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(ValidationHelper), nameof(ValidationHelper.ValidateDateOfBirth))]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public int CityId { get; set; }

        public List<PhoneNumberDTO> PhoneNumbers { get; set; } = new List<PhoneNumberDTO>();
    }
}
