using System.ComponentModel.DataAnnotations;

namespace Common.Dto
{
    public class CreateReviewDto
    {
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public int DeliveryDriverId { get; set; }
        public string ReviewText { get; set; }
        public int OrderId { get; set; }
        [Range(1, 5,
            ErrorMessage = "Value for {Rating} must be between {1} and {5}.")]
        public int Rating { get; set; }
    }
}
