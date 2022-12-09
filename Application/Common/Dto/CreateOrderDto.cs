using Common.Enums;

namespace Common.Dto
{
    public class CreateOrderDto
    {
        public PaymentTypes PaymentType { get; set; }
        public CardTypes? CardType { get; set; }
        public string VoucherCode { get; set; }
        public float OrderTotal { get; set; }
        public int RestaurantId { get; set; }
        public string CustomerEmail { get; set; }
        public List<CreateOrderMenuItemDto> MenuItems { get; set; } = new List<CreateOrderMenuItemDto>();
    }
}