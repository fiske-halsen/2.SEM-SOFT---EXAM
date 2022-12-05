using Common.Dto;
using Common.Enums;

namespace PaymentValidatorService.Services
{
    public class PaymentValidatorService
    {
        public interface IPaymentValidatorSerice {
            bool CheckPaymentType(CreateOrderDto dto);
            bool CheckForCardType(CreateOrderDto dto);
            void CheckForCardNumber(CreateOrderDto dto);
        }

        public PaymentValidatorService() {
        }

        //Check for payment types
        public bool CheckPaymentType(CreateOrderDto dto)
        {
            switch (dto.PaymentType)
            {
                case PaymentTypes.CreditCard:
                    return true;
                case PaymentTypes.Voucher:
                    return true;
                case PaymentTypes.UserCredit:
                    return true;
                default:
                    return false;
            }
        }

        //Check for card types
        public bool CheckForCardType(CreateOrderDto dto)
        {
            switch (dto.CardType) {
                case CardTypes.Visa:
                    return true;
                case CardTypes.MasterCard:
                    return true;
                case CardTypes.Debit:
                    return true;
                case CardTypes.Dankort:
                    return true;
                default:
                    return false;
            }
        }

        //Check for vouchers
        public void CheckForVouchers(CreateOrderDto dto)
        {
            switch (dto.Voucher)
            {
                case Vouchers.FiftyOff:
                    dto.Total = dto.Total / 2;
                    break;
                case Vouchers.FreeDelivery:
                    dto.FreeDelivery = true;
                    break;
                case Vouchers.FreeFood:
                    dto.Total = 0;
                    break;
                default:
                    return;
            }
        }
    }
}
