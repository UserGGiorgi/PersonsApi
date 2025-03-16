using PersonsApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace PersonsApi.Models
{
    public class PhoneNumber
    {
        public int Id { get; set; }

        public PhoneNumberType NumberType { get; set; }

        public string Number { get; set; } = null!;

        public int PersonId { get; set; }

        public Person Person { get; set; } = null!;
    }
}
