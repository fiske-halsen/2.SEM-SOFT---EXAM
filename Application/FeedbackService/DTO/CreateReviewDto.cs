namespace FeedbackService.DTO
{
    public class CreateReviewDto
    {
        public string ReviewText { get; set; }
        public DateTime ReviewDate { get; set; }
        public int OrderId { get; set; }
        public int Rating { get; set; }

    }
}
