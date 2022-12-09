namespace Common.Dto
{
    public class ApproveOrderDto
    {
        public string CustomerEmail { get; set; }
        public int OrderId { get; set; }
        public List<int> MenuItemsIds { get; set; } = new List<int>();
    }
}