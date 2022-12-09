namespace Common.Dto
{
    public class DeliveryDto
    {
        public int DeliveryId { get; set; }
        public int DeliveryPersonId { get; set; }
        public int OrderId { get; set; }
        public int RestaurantId { get; set; }
        public string UserEmail { get; set; }
        public bool IsDelivered { get; set; } 
        public DateTime TimeToDelivery { get; set; }
        public DateTime CreatedDate { get; set; } 
    }
}
