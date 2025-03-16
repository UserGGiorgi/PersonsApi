using Microsoft.EntityFrameworkCore;
using PersonsApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace PersonsApi.Models
{
    public class Person
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public Gender Gender { get; set; }

        public string PersonalNumber { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        public int CityId { get; set; }

        public City City { get; set; } = null!;
        public List<PhoneNumber> PhoneNumbers { get; set; } = new List<PhoneNumber>();

        public string? Image { get; set; }

        public List<RelatedIndividual> RelatedIndividuals { get; set; } = new List<RelatedIndividual>();

    }
}
