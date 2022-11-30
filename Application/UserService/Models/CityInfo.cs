namespace UserService.Models
{
    public class CityInfo
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}
