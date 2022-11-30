using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace RestaurantService.Model
{
    public class Restaurant
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Name { get; set; }
        
        [ForeignKey("AddressId")]
        public int AddressId { get; set; }
        public Address Address { get; set; }
        
        [ForeignKey("MenuId")]
        public int MenuId { get; set; }
        public Menu Menu { get; set; }
    }
}
