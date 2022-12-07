using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantService.Model
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        
        public int StockCount { get; set; }
        [ForeignKey("MenuId")]
        public int MenuId { get; set; }
        public Menu Menu { get; set; }
    }
}
