namespace Common.Dto
{
    public class OrderDeliveredDto
    {
        public int OrderId { get; set; }
        public int DeliveryId { get; set; }
        public string UserEmail { get; set; }
    }
}
