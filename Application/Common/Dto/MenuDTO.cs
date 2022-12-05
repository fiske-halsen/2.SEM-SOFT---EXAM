using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
    public class MenuDTO
    {
        public string RestaurantName { get; set; }
        public List<MenuItemDTO> MenuItems { get; set; } = new List<MenuItemDTO>();
    }
}
