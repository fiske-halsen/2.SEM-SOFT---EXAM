using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantService.Model
{
    public class Address
    {
        public int Id { get; set; }
        public string StreetName { get; set; }
        [ForeignKey("CityInfoId")]
        public int CityInfoId { get; set; }
        public CityInfo CityInfo { get; set; }
    }
}
