using Common.Models;
using Bogus;
using Microsoft.AspNetCore.Http.Features;
using System.Runtime.CompilerServices;

namespace GraphqlDemo.Operations
{
    public class Query
    {
        public List<Order> GetOrder()
        {
            Faker<MenuItem> menuItemFaker = new Faker<MenuItem>()
                .StrictMode(true)
                .RuleFor(x => x.Id, x => x.Random.Int())
                .RuleFor(x => x.Price, x => x.Random.Int())
                .RuleFor(x => x.Name, x => x.Name.FirstName());

            Faker<Order> orderFaker = new Faker<Order>()
                .StrictMode(true)
                .RuleFor(x => x.Id, x => Guid.NewGuid())
                .RuleFor(x => x.Customer, x => x.Person.FullName)
                .RuleFor(x => x.Restaurant, x => x.Company.CompanyName())
                .RuleFor(x => x.Total, x => x.Random.Int())
                .RuleFor(x => x.Items, menuItemFaker.Generate(10));

            return orderFaker.Generate(10);
        }

    }
}