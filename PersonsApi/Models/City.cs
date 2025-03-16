using System.ComponentModel.DataAnnotations;

namespace PersonsApi.Models
{
    public class City
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;    

        public string Country { get; set; } = null!;
    }
}
