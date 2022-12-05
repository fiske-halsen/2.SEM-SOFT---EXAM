namespace DeliveryService.DTO
{
    public class CreateDeliveryDTO
    {
        public int DeliveryPersonId { get; set; }
        public int OrderId { get; set; }
        public string UserEmail { get; set; }
    }
}
