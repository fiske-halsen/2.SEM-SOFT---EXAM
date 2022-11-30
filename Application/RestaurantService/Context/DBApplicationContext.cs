using Microsoft.EntityFrameworkCore;
using RestaurantService.Model;

namespace RestaurantService.Context
{
    public class DBApplicationContext : DbContext
    {
        public DBApplicationContext(DbContextOptions<DBApplicationContext> options) : base(options) { }
        public DbSet<Menu> Menus { get; set; }
        public DbSet <MenuItem> MenuItems { get; set; }
        public DbSet <Restaurant> Restaurants { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<CityInfo> CityInfos { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            Seed(builder);
            base.OnModelCreating(builder);
        }
        private void Seed(ModelBuilder builder)
        {
            builder.Entity<CityInfo>().HasData(
                new CityInfo { Id = 1, City = "Gentofte", ZipCode = "2920" }

                );
            builder.Entity<Address>().HasData(
                new Address { Id = 1, CityInfoId = 1, StreetName = "Skovvej" },
                new Address { Id = 2, CityInfoId = 1, StreetName = "Hovmarksvej" }

                );
            builder.Entity<Menu>().HasData(
                new Menu { Id = 1 },
                new Menu { Id = 2 }

                );
            builder.Entity<Restaurant>().HasData(
               new Restaurant { AddressId = 1, Id = 1, Name = "PizzaPusheren", MenuId = 1, OwnerId = 1 },
               new Restaurant { AddressId = 2, Id = 2, Name = "SushiSlyngeren", MenuId = 2, OwnerId = 2 }
           );
            builder.Entity<MenuItem>().HasData(
                 new MenuItem { Id = 1, Name = "salatpizza",  Price = 79.99, MenuId = 1, Description = "wow smager godt" },
                new MenuItem { Id = 2, Name = "Peperoni", Price = 79.23, MenuId = 1, Description = "wow smager godt" },
                new MenuItem { Id = 3, Name = "Calzone", Price = 89.99, MenuId = 1, Description = "wow smager godt" },
                new MenuItem { Id = 4, Name = "ChokoladeIs",  Price = 39.99, MenuId = 1 , Description = "wow smager godt" },
                new MenuItem { Id = 5, Name = "vaniljeis",  Price = 39.99, MenuId = 1, Description = "wow smager godt" },
                new MenuItem { Id = 6, Name = "chokoladekage",  Price = 39.99, MenuId = 1, Description = "wow smager godt" },
                new MenuItem { Id = 7, Name = "Cola", Price = 19.99, MenuId = 1, Description = "wow smager godt" },
                new MenuItem { Id = 8, Name = "Fanta",  Price = 19.99, MenuId = 1, Description = "wow smager godt" },
                new MenuItem { Id = 9, Name = "Mayo",  Price = 9.99, MenuId = 1 , Description = "wow smager godt" },
                new MenuItem { Id = 10, Name = "Ketchup",  Price = 9.99, MenuId = 1 , Description = "wow smager godt" }
                );


        }
    }
}
