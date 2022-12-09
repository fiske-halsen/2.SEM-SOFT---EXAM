using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BoDi;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using Microsoft.Extensions.Configuration;

namespace RestaurantService.Test.Hooks
{
    [Binding]
    public class DockerControllerHooks
    {
        private static ICompositeService _compositeService;
        private IObjectContainer _objectContainer;

        [BeforeTestRun]
        public static void DockerComposeUp()
        {
            var config = LoadConfiguration();

            var dockerComposeFileName = config["DockerComposeFileName"];
            var dockerComposePath = GetDockerComposeLocation(dockerComposeFileName);

            var confirmationUrl = config["RestaurantService.Api:BaseAddress"];


            _compositeService = new Builder().UseContainer().UseCompose().FromFile(dockerComposePath).RemoveOrphans()
                .WaitForHttp("webapi", $"{confirmationUrl}/Restaurant",
                    continuation: (response, _) => response.Code != HttpStatusCode.OK ? 2000 : 0).Build().Start();
        }

        private static string GetDockerComposeLocation(string dockerComposeFileName)
        {
            var directory = Directory.GetCurrentDirectory();
            while (!Directory.EnumerateFiles(directory, "*se.yml").Any(s => s.EndsWith(dockerComposeFileName)))
            {
                directory = directory.Substring(0, directory.LastIndexOf(Path.DirectorySeparatorChar));

            }

            return Path.Combine(directory, dockerComposeFileName);
        }

        private static IConfiguration LoadConfiguration()
        {
            return new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }
        [BeforeScenario()]
        public void AddHttpClient()
        {
            var config = LoadConfiguration();
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(config["RestaurantService.Api:BaseAddress"])
            };
            _objectContainer.RegisterInstanceAs(httpClient);
        }

        [AfterTestRun]
        public static void DockerComposeDown()
        {
            
            
        }
    }
}
