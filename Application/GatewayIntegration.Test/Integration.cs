using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;

namespace GatewayIntegration.Test
{
    public class Integration
    {
        //private WebApplicationFactory<Program> _webApplicationFactoryGateWay;
        //private HttpClient _httpClientGateWay;

        //private WebApplicationFactory<Program> _webApplicationFactoryRestaurant;
        //private HttpClient _httpClientRestaurant;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
          //  _webApplicationFactoryGateWay = new WebApplicationFactory<Program>();
           // _httpClientGateWay = _webApplicationFactoryGateWay.CreateClient();




            //_webApplicationFactoryRestaurant = new WebApplicationFactory<Program>();
            //_httpClientRestaurant = _webApplicationFactoryRestaurant.CreateClient();
            // mock identity server
            // disable auth på endpoints til graphql
            // Start restaurantService i container
            // Start restaurantDB i container
        }

        [Test]
        public void Test1()
        {
            //_httpClient.GetAsync("restaurant/menuitem")

            Assert.Pass();

        }


        [OneTimeTearDown]
        public void OneTimeTearDown()
        {

        }
    }
}