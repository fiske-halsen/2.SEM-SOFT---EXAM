using Common.Dto;

namespace PaymentValidatorService.Services
{
    public interface IPaymentValidatorService
    {
        public Task<bool> ValidatePayment(CreateOrderDto createOrderDto);
    }

    public class PaymentValidatorService : IPaymentValidatorService
    {
        private readonly IPaymentValidatorHelpers _paymentHelper;

        public PaymentValidatorService(IPaymentValidatorHelpers paymentHelper)
        {
            _paymentHelper = paymentHelper;
        }

        /// <summary>
        /// Validates the actual payment
        /// </summary>
        /// <param Name="createOrderDto"></param>
        /// <returns></returns>
        public async Task<bool> ValidatePayment(CreateOrderDto createOrderDto)
        {
            return _paymentHelper.CheckPaymentType(createOrderDto) &&
                   _paymentHelper.CheckForCardType(createOrderDto) &&
                   _paymentHelper.CheckForValidVoucherCode(createOrderDto.VoucherCode);
        }
    }
}