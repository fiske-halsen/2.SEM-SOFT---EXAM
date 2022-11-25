namespace FeedbackService.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Restaurant{ get; set; }
        public string ReviewText{ get; set; }
        public DateTime ReviewDate{ get; set; }
        public int Rating{ get; set; }

    }
}
