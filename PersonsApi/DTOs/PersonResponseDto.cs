using PersonsApi.Dtos;
using PersonsApi.Enums;
using PersonsApi.Models;
using System.ComponentModel.DataAnnotations;

namespace PersonsApi.DTOs
{
    public class PersonResponseDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;
        public Gender Gender { get; set; }

        public string PersonalNumber { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }
        public int CityId { get; set; }

        public City City { get; set; } = null!;
        public List<PhoneNumberDTO> PhoneNumbers { get; set; } = new List<PhoneNumberDTO>();

        public string? Image { get; set; }

        public List<RelatedIndividualDTO> RelatedIndividuals { get; set; } = new List<RelatedIndividualDTO>();
    }
}
