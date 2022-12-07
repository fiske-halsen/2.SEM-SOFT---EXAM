using Common.Enums;

namespace Common.Dto
{
    public class OrderDto
    {
        public int Id { get; set; }
        public double TotalPrice { get; set; }
        public int RestaurantId { get; set; }
        public string CustomerEmail { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsApproved { get; set; } = false;
        public PaymentTypes PaymentType { get; set; }
        public CardTypes? CardType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}