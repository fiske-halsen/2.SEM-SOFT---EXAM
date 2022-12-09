using System.ComponentModel.DataAnnotations;

namespace DeliveryService.Models
{
    public class Delivery
    {
        public int DeliveryId { get; set; }
        public int DeliveryPersonId { get; set; }
        public int OrderId { get; set; }
        public int RestaurantId { get; set; }
        [Required]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.(com|net|org|gov|dk)$")]
        public string UserEmail { get; set; }
        public bool IsDelivered { get; set; } = false;
        public DateTime TimeToDelivery { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
