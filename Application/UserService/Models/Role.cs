using Common.Enums;

namespace UserService.Models
{
   

    public class Role
    {
        public int Id { get; set; }
        public Enums.RoleTypes RoleType { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
