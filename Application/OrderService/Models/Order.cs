using Common.Enums;

namespace OrderService.Models
{
    public class Order
    {
        public int Id { get; set; }
        public double TotalPrice { get; set; }
        public int RestaurantId { get; set; }
        public string CustomerEmail { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsApproved { get; set; } = false;
        public List<OrderItem> MenuItems { get; set; }
        public PaymentTypes PaymentType { get; set; }
        public CardTypes? CardType { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}