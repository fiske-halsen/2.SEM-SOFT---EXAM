using Common.Dto;
using Common.Enums;
using Common.KafkaEvents;
using Newtonsoft.Json;

namespace PaymentProcessorService.Services
{
    public interface IPaymentService
    {
        public Task<bool> SimulatePayment(CreateOrderDto createOrderDto);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IKafkaPaymentProcessorProducer _kafkaPaymentProcessorProducer;
        private readonly IPaymentProcessorHelpers _paymentProcessorHelper;

        public PaymentService(IKafkaPaymentProcessorProducer kafkaPaymentProcessor,
            IPaymentProcessorHelpers paymentProcessorHelper)
        {
            _kafkaPaymentProcessorProducer = kafkaPaymentProcessor;
            _paymentProcessorHelper = paymentProcessorHelper;
        }

        public async Task<bool> SimulatePayment(CreateOrderDto createOrderDto)
        {
            // Take off eventuel discount
            _paymentProcessorHelper.CheckForDiscountVouchers(createOrderDto);

            switch (createOrderDto.PaymentType)
            {
                case PaymentTypes.CreditCard: // Notify restaurant directly to check stock
                    await _kafkaPaymentProcessorProducer.ProduceToKafka(EventStreamerEvents.CheckRestaurantStockEvent,
                        JsonConvert.SerializeObject(createOrderDto));
                    break;
                case PaymentTypes.UserCredit: // Notify user service to update user credit 
                    await _kafkaPaymentProcessorProducer.ProduceToKafka(EventStreamerEvents.CheckUserBalanceEvent,
                        JsonConvert.SerializeObject(createOrderDto));
                    break;
                case PaymentTypes.Voucher: // Notify restaurant directly to check stock
                    await _kafkaPaymentProcessorProducer.ProduceToKafka(EventStreamerEvents.CheckRestaurantStockEvent,
                        JsonConvert.SerializeObject(createOrderDto));
                    break;
            }

            return true;
        }
    }
}