namespace RestaurantService.Model
{
    public class MenuDTO
    {
        public string RestaurantName{ get; set; }
        List<MenuItemDTO> MenuItems { get; set; } = new List<MenuItemDTO>();
    }
}
