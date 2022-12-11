namespace Common.Dto
{
    public class CreateMenuDto
    {
        public List<CreateMenuItemDto> MenuItems { get; set; } = new List<CreateMenuItemDto>();
    }
}
