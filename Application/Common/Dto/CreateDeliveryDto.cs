namespace Common.Dto
{
    public class CreateDeliveryDto
    {
        public int DeliveryPersonId { get; set; }
        public int OrderId { get; set; }
        public int RestaurantId { get; set; }
        public string UserEmail { get; set; }
        public DateTime TimeToDelivery { get; set; }
    }
}