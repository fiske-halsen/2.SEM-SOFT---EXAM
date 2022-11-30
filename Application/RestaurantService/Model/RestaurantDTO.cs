using System.ComponentModel.DataAnnotations;

namespace RestaurantService.Model
{
    public class RestaurantDTO
    {
        [Required]
        public string RestaurantName { get; set; }
        [Required]
        public string StreetName { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string ZipCode { get; set; }
    }
}
