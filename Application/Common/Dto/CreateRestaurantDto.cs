using System.ComponentModel.DataAnnotations;

namespace Common.Dto
{
    public class CreateRestaurantDto
    {
        [Required]
        public string RestaurantName { get; set; }
        [Required]
        public string StreetName { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]
        public CreateMenuDto Menu { get; set; }
    }
}
