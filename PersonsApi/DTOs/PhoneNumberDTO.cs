using PersonsApi.Attribute;
using PersonsApi.Enums;
using PersonsApi.Models;
using System.ComponentModel.DataAnnotations;

namespace PersonsApi.Dtos
{
    public class PhoneNumberDTO
    {
        [Required]
        public PhoneNumberType NumberType { get; set; }

        [Required]
        [CustomPhoneNumber(ErrorMessage = "The number must contain only digits and may start with a '+' sign")]
        public string Number { get; set; }
    }
}
