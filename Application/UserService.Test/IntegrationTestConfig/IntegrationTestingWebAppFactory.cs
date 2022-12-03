using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using UserService.Context;

namespace UserService.Test.IntegrationTestConfig
{
    public class IntegrationTestingWebAppFactory<TEntryPoint> : WebApplicationFactory<Program>
        where TEntryPoint : Program
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<UserDbContext>));


                services.Remove(descriptor);

                services.AddDbContext<UserDbContext>(options => { 
                    options.UseInMemoryDatabase("InMemoryUserTest"); });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())

                using (var appContext = scope.ServiceProvider.GetRequiredService<UserDbContext>())
                {
                    try
                    {
                        if (appContext.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
                        {
                            appContext.Database.Migrate();
                            appContext.Database.EnsureCreated();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                        throw;
                    }
                }

            });

        }
    }
}