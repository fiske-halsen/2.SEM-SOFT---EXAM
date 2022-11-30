namespace UserService.Models
{
    public enum RoleTypes // TODO move to COMMON when ready
    {
        Customer = 1,
        RestaurantOwner = 2,
        DeliveryPerson = 3
    }

    public class Role
    {
        public int Id { get; set; }
        public RoleTypes RoleType { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
