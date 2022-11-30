using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [ForeignKey("AddressId")]
        public int AddressId { get; set; }
        public Address Address { get; set; }
        [ForeignKey("RoleId")]
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    }
}
