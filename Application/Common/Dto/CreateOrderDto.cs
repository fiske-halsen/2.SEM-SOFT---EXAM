using Common.Models;

namespace Common.Dto
{
    public class CreateOrderDto
    {
        // Not sure what should go in here yet
        public PaymentTypes PaymentType { get; set; }
        public CardTypes? CardType { get; set; }
        public Vouchers? Voucher { get; set; }
        public List<MenuItem> Items { get; set; } = new List<MenuItem>();
        public float Total { get; set; }
        public int RestaurantId { get; set; }
        public string CustomerEmail { get; set; }
        public bool FreeDelivery { get; set; }
        public List<MenuItemDTO> MenuItems { get; set; }

    }
}