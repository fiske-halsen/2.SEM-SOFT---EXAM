using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string StreetName { get; set; }
        public string HouseNumber { get; set; }
        public int? Floor { get; set; }
        public string? DoorDesignation { get; set; }
        [ForeignKey("CityInfoId")]
        public int CityInfoId { get; set; }
        public CityInfo CityInfo { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
