namespace FeedbackService.Models
{
        public class Complaint
        {
        public int id { get; set; }
        public string name { get; set; }
            public string restaurant { get; set; }
            public string complaintText { get; set; }
            public DateTime complaintDate { get; set; }
            public int orderId { get; set; }

        }
    
}
