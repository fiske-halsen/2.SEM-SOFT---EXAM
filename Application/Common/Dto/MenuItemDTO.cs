using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
    public class MenuItemDTO
    {
        public int Id { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public string description { get; set; }
        public int StockCount { get; set; }
        
    }
}
