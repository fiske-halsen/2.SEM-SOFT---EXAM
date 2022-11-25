namespace FeedbackService.Models
{
        public class Complaint
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Restaurant { get; set; }
            public string ComplaintText { get; set; }
            public DateTime ComplaintDate { get; set; }
            public int OrderId { get; set; }

        }
    
}
