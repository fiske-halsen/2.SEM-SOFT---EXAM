namespace Common.KafkaEvents
{
    public static class EventStreamerEvents
    {
        public const string ValidatePayment = "validate_payment"; 
        public const string ValidPaymentEvent = "valid_payment";
        public const string NotifyUserEvent = "notify_user";
        public const string CheckUserBalanceEvent = "check_user_balance";
        public const string CheckRestaurantStockEvent = "check_restaurant_stock";
        public const string UpdateRestaurantStockEvent = "update_restaurant_stock";
        public const string SaveOrderEvent = "save_order";
        public const string ApproveOrderEvent = "approve_order";
    }
}
