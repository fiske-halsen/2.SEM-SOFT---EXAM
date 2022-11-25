﻿namespace FeedbackService.Models
{
    public class Review
    {
        public int id { get; set; }
        public string name { get; set; }
        public string restaurant{ get; set; }
        public string reviewText{ get; set; }
        public DateTime reviewDate{ get; set; }
        public int rating{ get; set; }

    }
}
