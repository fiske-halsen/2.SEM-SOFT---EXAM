namespace RestaurantService.Model
{
    public class Menu
    {
        public int Id { get; set; }
        public List<MenuItem> MenuItems = new List<MenuItem>();
        public Restaurant Restaurant { get; set; }
    }
}
