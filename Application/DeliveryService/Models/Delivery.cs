namespace DeliveryService.Models
{
    public class Delivery
    {
        public int DeliveryId { get; set; }
        public int DeliveryPersonId { get; set; }
        public int OrderId { get; set; }
        public string UserEmail { get; set; }
        public bool IsDelivered { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
