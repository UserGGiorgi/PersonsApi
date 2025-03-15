using PersonsApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace PersonsApi.Models
{
    public class RelatedIndividual
    {
        public int Id { get; set; }

        public RelationshipType RelationshipType { get; set; }

        public int PersonId { get; set; }

        public Person Person { get; set; }

        public int RelatedPersonId { get; set; }

        public Person RelatedPerson { get; set; }
    }
}
