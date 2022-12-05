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
                        createOrderDto.OrderTotal = createOrderDto.OrderTotal * 0.85f;
                        break;
                    case "25off":
                        createOrderDto.OrderTotal = createOrderDto.OrderTotal * 0.75f;
                        break;
                    case "50off":
                        createOrderDto.OrderTotal = createOrderDto.OrderTotal * 0.50f;
                        break;
                    case "100off":
                        createOrderDto.OrderTotal = 0; // In this case its free
                        break;
                    default:
                        return;
                }
            }
        }
    }
}