namespace Common.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public List<MenuItem> Items { get; set; } = new List<MenuItem>();
        public float Total { get; set; }
        public string Restaurant { get; set; }
        public string Customer { get; set; }
    }
}