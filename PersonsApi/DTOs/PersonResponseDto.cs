using PersonsApi.Dtos;
using PersonsApi.Enums;
using PersonsApi.Models;
using System.ComponentModel.DataAnnotations;

namespace PersonsApi.DTOs
{
    public class PersonResponseDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public Gender Gender { get; set; }

        public string PersonalNumber { get; set; }

        public DateTime DateOfBirth { get; set; }
        public int CityId { get; set; }

        public City City { get; set; }
        public List<PhoneNumberDTO> PhoneNumbers { get; set; } = new List<PhoneNumberDTO>();

        public string Image { get; set; }

        public List<RelatedIndividualDTO> RelatedIndividuals { get; set; } = new List<RelatedIndividualDTO>();

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
