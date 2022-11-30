namespace Gateway.Models
{
    public class Order
    {
        public int Id { get; set; }
        public List<MenuItem> Items { get; set; }
        public double Total { get; set; }
        public string Restaurant { get; set; }
        public string Customer { get; set; }
    }
}
