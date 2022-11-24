namespace OrderService.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Item> MenuItems { get; set; }
        public string TotalPrice { get; set; }
        public string Restaurant { get; set; }
        public string Customer { get; set; }
    }
}
