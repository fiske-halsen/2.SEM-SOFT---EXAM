using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BoDi;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;

namespace RestaurantServiceIntegration.Test
{
    public class Hook
    {
        private static ICompositeService _compositeService;
        private IObjectContainer _objectContainer;

        public Hook(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        public  void DockerComposeUp()
        {
            var config = LoadConfiguration();

            var dockerComposeFileName = config["DockerComposeFileName"];
            var dockerComposePath = GetDockerComposeLocation(dockerComposeFileName);

            var confirmationUrl = config["RestaurantService.Api:BaseAddress"];


            _compositeService = new Builder().UseContainer().UseCompose().FromFile(dockerComposePath).RemoveOrphans()
                .WaitForHttp("webapi", $"{confirmationUrl}/Restaurant",
                    continuation: (response, _) => response.Code != HttpStatusCode.OK ? 2000 : 0).Build().Start();
        }

        private  string GetDockerComposeLocation(string dockerComposeFileName)
        {
            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            return di.Parent.Parent.Parent.Parent.ToString() + "\\docker-compose.yml";
        }

        private  IConfiguration LoadConfiguration()
        {
            return new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }
        

        public  void DockerComposeDown()
        {
            _compositeService.Stop();
            //_compositeService.Dispose();

        }
    }
}
