namespace Common.Dto
{
    public class MenuDTO
    {
        public string RestaurantName { get; set; }
        public List<MenuItemDTO> MenuItems { get; set; } = new List<MenuItemDTO>();
    }
}
