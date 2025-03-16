using System.ComponentModel.DataAnnotations;

namespace PersonsApi.DTOs
{
    public class CityDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        public string Country { get; set; } = null!;
    }
}
