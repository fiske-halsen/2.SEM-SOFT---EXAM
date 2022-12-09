using System.ComponentModel.DataAnnotations;

namespace Common.Dto
{
    public class UpdateUserBalanceDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public double Balance { get; set; }
    }
}
