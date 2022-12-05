using Common.Dto;
using Common.Enums;
using Newtonsoft.Json;

namespace PaymentValidatorService.Services
{
    public interface IPaymentValidatorService
    {
        public Task<bool> ValidatePayment(CreateOrderDto createOrderDto);
    }

    public class PaymentValidatorService : IPaymentValidatorService
    {
        private readonly IPaymentValidatorHelpers _paymentHelper;
        private readonly IPaymentValidatorProducer _kafkaProducer;

        public PaymentValidatorService(IPaymentValidatorHelpers paymentHelper, IPaymentValidatorProducer kafkaProducer)
        {
            _paymentHelper = paymentHelper;
            _kafkaProducer = kafkaProducer;
        }

        /// <summary>
        /// Validates the actual payment
        /// </summary>
        /// <param name="createOrderDto"></param>
        /// <returns></returns>
        public async Task<bool> ValidatePayment(CreateOrderDto createOrderDto)
        {
            var isValidPaymentType = _paymentHelper.CheckPaymentType(createOrderDto);
            var isValidCreditCapeType = _paymentHelper.CheckForCardType(createOrderDto);
            var isValidVoucher = _paymentHelper.CheckForValidVoucherCode(createOrderDto.VoucherCode);

            if (isValidCreditCapeType && isValidPaymentType && isValidVoucher)
            {
                // Produce new event to kafka in case of valid payment types
                await _kafkaProducer.ProduceToKafka("valid_payment", JsonConvert.SerializeObject(createOrderDto));
                return true;
            }
            // Notify hub using web sockets

            return false;
        }
    }
}
