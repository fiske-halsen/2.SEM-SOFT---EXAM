using Common.Dto;
using Common.Enums;
using Common.KafkaEvents;
using Common.KafkaProducer;
using Newtonsoft.Json;

namespace PaymentProcessorService.Services
{
    public interface IPaymentService
    {
        public Task<bool> SimulatePayment(CreateOrderDto createOrderDto, IGenericKafkaProducer kafkaProducer);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IPaymentProcessorHelpers _paymentProcessorHelper;

        public PaymentService(IPaymentProcessorHelpers paymentProcessorHelper)
        {
            _paymentProcessorHelper = paymentProcessorHelper;
        }

        public async Task<bool> SimulatePayment(CreateOrderDto createOrderDto, IGenericKafkaProducer kafkaProducer)
        {
            // Take off eventuel discount
            _paymentProcessorHelper.CheckForDiscountVouchers(createOrderDto);

            switch (createOrderDto.PaymentType)
            {
                case PaymentTypes.CreditCard: // Notify restaurant directly to check stock
                    await kafkaProducer.ProduceToKafka(EventStreamerEvents.CheckRestaurantStockEvent,
                        JsonConvert.SerializeObject(createOrderDto));
                    break;
                case PaymentTypes.UserCredit: // Notify user service to update user credit 
                    await kafkaProducer.ProduceToKafka(EventStreamerEvents.CheckUserBalanceEvent,
                        JsonConvert.SerializeObject(createOrderDto));
                    break;
                case PaymentTypes.Voucher: // Notify restaurant directly to check stock
                    await kafkaProducer.ProduceToKafka(EventStreamerEvents.CheckRestaurantStockEvent,
                        JsonConvert.SerializeObject(createOrderDto));
                    break;
            }

            return true;
        }
    }
}