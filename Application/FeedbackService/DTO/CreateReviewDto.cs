namespace FeedbackService.DTO
{
    public class CreateReviewDto
    {
        public int Id { get; set; }
        public int UserID { get; set; }
        public int RestaurantId { get; set; }
        public int DeliveryDriverId { get; set; }
        public string ReviewText { get; set; }
        public DateTime ReviewDate { get; set; }
        public int Rating { get; set; }
    }
}
