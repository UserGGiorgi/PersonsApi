using PersonsApi.Enums;
using PersonsApi.Models;
using System.ComponentModel.DataAnnotations;

namespace PersonsApi.Dtos
{
    public class RelatedIndividualDTO
    {
        [Required]
        public RelationshipType RelationshipType { get; set; }

        [Required]
        public int RelatedPersonId { get; set; }
    }
}
