﻿using Common.Dto;
using Common.DummyData;
using Common.Enums;

namespace PaymentValidatorService.Services
{
    public interface IPaymentValidatorHelpers
    {
        public bool CheckPaymentType(CreateOrderDto dto);
        public bool CheckForCardType(CreateOrderDto dto);
        public bool CheckForValidVoucherCode(string voucherCode);
    }

    public class PaymentValidatorHelpers : IPaymentValidatorHelpers
    {
        /// <summary>
        /// Simulates valid credit card types
        /// </summary>
        /// <param Name="dto"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Simulate valid card types
        /// </summary>
        /// <param Name="dto"></param>
        /// <returns></returns>
        public bool CheckForCardType(CreateOrderDto dto)
        {
            if (dto.CardType.HasValue)
            {
                switch (dto.CardType)
                {
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

            return true; // meaning no credit card payment
        }

        public bool CheckForValidVoucherCode(string voucherCode)
        {
            if (!String.IsNullOrEmpty(voucherCode))
            {
                return DiscountVouchers.VoucherCodes.Contains(voucherCode);
            }

            return true; // Meaning no voucher code was typed in
        }
    }
}