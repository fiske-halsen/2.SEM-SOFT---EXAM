namespace FeedbackService.Models
{
        public class Complaint
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public int RestaurantId { get; set; }
            public int DeliveryDriverID { get; set; }
            public string ComplaintText { get; set; }
            public DateTime ComplaintDate { get; set; }
            public int OrderId { get; set; }

        }
    
}
