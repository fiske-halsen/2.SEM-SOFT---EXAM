using BoDi;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantServiceIntegration.Test.Integration
{
    [TestFixture]
    public class IntegrationTests
    {
        private HttpClient _httpClient;
        private WebApplicationFactory<Program> _webApplicationFactory;
        private Hook _hook;
        private IObjectContainer _objectContainer;

        [OneTimeSetUp]
        public void Setup()
        {
            _hook = new Hook(_objectContainer);
            _hook.DockerComposeUp();
            _webApplicationFactory = new WebApplicationFactory<Program>();
            _httpClient = _webApplicationFactory.CreateClient();

        }

        [OneTimeTearDown]
        public void AfterTestRun()
        {
            _hook.DockerComposeDown();
        }
        /// <summary>
        /// Test for a user finding a given menu-item from a given restaurant
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Test_Get_Menu_Item_Returns_Correct_Menu_Item()
        {


            var response = await _httpClient.GetAsync($"/1/menu-item/1");
            var stringResult = await response.Content.ReadAsStringAsync();
            var expected = "{\"id\":1,\"name\":\"salatpizza\",\"price\":79.99,\"description\":\"wow smager godt\",\"stockCount\":10}";

            Assert.AreEqual(expected, stringResult);
        }
        [Test]
        public async Task Test_Get_Restaurant_Menu_Returns_Correct_Menu()
        {


            var response = await _httpClient.GetAsync($"/1/menu-item/1");
            var stringResult = await response.Content.ReadAsStringAsync();
            var expected = "{\"id\":1,\"name\":\"salatpizza\",\"price\":79.99,\"description\":\"wow smager godt\",\"stockCount\":10}";

            Assert.AreEqual(expected, stringResult);
        }
    }
}
