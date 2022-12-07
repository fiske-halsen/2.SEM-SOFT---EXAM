using Bogus;
using Common.Dto;

namespace GraphqlDemo.Operations
{
    public class Query
    {
        public List<MenuDTO> GetOrder()
        {
            //Faker<MenuItem> menuItemFaker = new Faker<MenuItem>()
            //    .StrictMode(true)
            //    .RuleFor(x => x.Id, x => x.Random.Int())
            //    .RuleFor(x => x.Price, x => x.Random.Int())
            //    .RuleFor(x => x.Name, x => x.Name.FirstName());

            Faker<MenuDTO> orderFaker = new Faker<MenuDTO>()
                .RuleFor(x => x.RestaurantName, x => x.Name.FirstName());

            return orderFaker.Generate(10);
            return null;
        }

    }
}