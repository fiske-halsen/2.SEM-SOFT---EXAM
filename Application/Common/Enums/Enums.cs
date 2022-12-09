using System.Runtime.Serialization;

namespace Common.Enums
{
    #region Enums

    public enum PaymentTypes
    {
        CreditCard = 1,
        Voucher = 2,
        UserCredit = 3,
    }

    public enum RoleTypes
    {
        Customer = 1,
        RestaurantOwner = 2,
        DeliveryPerson = 3
    }

    public enum CardTypes
    {
        Visa = 1,
        MasterCard = 2,
        Debit = 3,
        Dankort = 4
    }

    public enum Vouchers
    {
        [EnumMember(Value = "fifty%OffFood")] FiftyOff = 1,

        [EnumMember(Value = "FreeDeliveryFood")]
        FreeDelivery = 2,

        [EnumMember(Value = "FreeFoodForPoorPeople")]
        FreeFood = 3
    }

    #endregion
}