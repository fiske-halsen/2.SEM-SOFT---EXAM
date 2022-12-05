using Common.Dto;

namespace PaymentProcessorService.Services
{
    public interface IPaymentProcessorHelpers
    {
        public void CheckForDiscountVouchers(CreateOrderDto createOrderDto);
    }

    public class PaymentProcessorHelpers : IPaymentProcessorHelpers
    {
        public void CheckForDiscountVouchers(CreateOrderDto createOrderDto)
        {
            if (!String.IsNullOrEmpty(createOrderDto.VoucherCode))
            {
                switch (createOrderDto.VoucherCode)
                {
                    case "15off":
                        createOrderDto.Total = createOrderDto.Total * 0.85f;
                        break;
                    case "25off":
                        createOrderDto.Total = createOrderDto.Total * 0.75f;
                        break;
                    case "50off":
                        createOrderDto.Total = createOrderDto.Total * 0.50f;
                        break;
                    case "100off":
                        createOrderDto.Total = 0; // In this case its free
                        break;
                    default:
                        return;
                }
            }
        }
    }
}