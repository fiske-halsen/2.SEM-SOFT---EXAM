namespace RestaurantService.Model
{
    public class Menu
    {
        public int Id { get; set; }
        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        public Restaurant Restaurant { get; set; }
    }
}
