namespace OrderService.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public List<Order> Orders { get; set; }
    }
}
