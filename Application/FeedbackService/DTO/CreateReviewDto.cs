namespace FeedbackService.DTO
{
    public class CreateReviewDTO
    {
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public int DeliveryDriverId { get; set; }
        public string ReviewText { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.UtcNow;
        public int OrderId { get; set; }
        public int Rating { get; set; }

    }
}
