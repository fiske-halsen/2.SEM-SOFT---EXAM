using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Common.Dto
{
    public class CreateUserDto
    {
        [JsonProperty("fName")]
        [Required(ErrorMessage = "First Name is required")]
        [MaxLength(length:15)]
        [MinLength(length:2)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        [JsonProperty("e")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MaxLength(length: 20)]
        [MinLength(length: 6)]
        [JsonProperty("p1")]

        public string Password { get; set; }

        [Required(ErrorMessage = "Password repeated is required")]
        [DataType(DataType.Password)]
        [MaxLength(length: 20)]
        [MinLength(length: 6)]
        [JsonProperty("p2")]
        public string PasswordRepeated { get; set; }
        [JsonProperty("sn")]
        [Required(ErrorMessage = "Streetname is required")]
        public string StreetName { get; set; }
        [Required(ErrorMessage = "City is required")]
        [JsonProperty("c")]
        public string City { get; set; }
        [Required(ErrorMessage = "Zip code is required")]
        [JsonProperty("z")]
        public string ZipCode { get; set; }

    }
}