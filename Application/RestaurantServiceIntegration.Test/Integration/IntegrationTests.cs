using BoDi;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Common.Dto;
using Common.ErrorModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestaurantService.Context;
using RestaurantService.Model;
using TechTalk.SpecFlow;
using Microsoft.AspNetCore.Builder;

namespace RestaurantServiceIntegration.Test.Integration
{
    [TestFixture]
    public class IntegrationTests
    {
        private HttpClient _httpClient;
        private WebApplicationFactory<Program> _webApplicationFactory;
        private Hook _hook;
        private IObjectContainer _objectContainer;
        private DBApplicationContext _dbContext;

        //public IntegrationTests(DBApplicationContext dbContex)
        //{
        //    _dbContext = dbContex;
        //}

        [OneTimeSetUp]
        public void OnetimeSetup()
        {
            
            _hook = new Hook(_objectContainer);
            _hook.DockerComposeUp();
            _webApplicationFactory = new WebApplicationFactory<Program>();
            _httpClient = _webApplicationFactory.CreateClient();
            var options = new DbContextOptionsBuilder<DBApplicationContext>()
                .UseSqlServer("Server=127.0.0.1,5438;Database=Restaurants;User Id=sa;Password=S3cur3P@ssW0rd!;TrustServerCertificate=True;").Options;
            _dbContext = new DBApplicationContext(options);
            
            _dbContext.Database.Migrate();
            _dbContext.Database.EnsureCreated();

        }

        [OneTimeTearDown]
        public void AfterTestRun()
        {
            _dbContext.Database.EnsureDeleted();
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
            var stringResult = await response.Content.ReadFromJsonAsync<MenuItemDTO>();
            var expected = new MenuItemDTO
            {
                Id = 1,
                Name = "salatpizza",
                Price = 79.99,
                Description = "wow smager godt",
                StockCount = 10
            };
                

            Assert.AreEqual(expected.Name, stringResult.Name);
        }
        /// <summary>
        /// Test for user to get a restaurants menu
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Test_Get_Restaurant_Menu_Returns_Correct_Menu()
        {


            var response = await _httpClient.GetAsync($"/1/menu");
            var stringResult = await response.Content.ReadFromJsonAsync<MenuDTO>();

            var expected = new MenuDTO()
            {
                RestaurantName = "PizzaPusheren"
            };
           
            Assert.AreEqual(expected.RestaurantName, stringResult.RestaurantName);
        }

        [Test]
        public async Task Test_Create_Menu_Item_Creates_Menu_Item()
        {
            var toBeCreatedMenuItem = new MenuItemDTO
            {
                Name = "salatpizza",
                Price = 79.99,
                Description = "wow smager godt",
                StockCount = 10
            };
            var content = JsonConvert.SerializeObject(toBeCreatedMenuItem);
            var stringContent = new StringContent(content, UnicodeEncoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"/1/menu-item", stringContent);
            var stringResult = await response.Content.ReadAsStringAsync();
            
            Assert.AreEqual(stringResult, "true");

        }

        [Test]
        public async Task Test_Delete_Menu_Item_Deletes_Menu_Item()
        {
            var response = await _httpClient.DeleteAsync("/1/menu-item/6");
            var stringResult = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(stringResult, "true");

        }
        //[Test]
        [Test]
        public async Task Negative_Test_Delete_Menu_Item_Deletes_Menu_Item()
        {
            var response = await _httpClient.DeleteAsync("/1/menu-item/9999");
            var stringResult = await response.Content.ReadFromJsonAsync<ExceptionDto>();
            var expected = new ExceptionDto()
            {
                Message = "Internal Server Error.",
                StatusCode = 500
            };

            Assert.AreEqual(stringResult.Message, expected.Message);

        }
        [Test]
        public async Task Test_Update_Menu_Item_Updates_Menu_Iten()
        {
            var newMenuItem = new MenuItem()
            {
                Id = 5, Description = "Updated Description", Name = "Updated Name", StockCount = 10
            };
            var content = JsonConvert.SerializeObject(newMenuItem);
            var stringContent = new StringContent(content, UnicodeEncoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/1/menu-item", stringContent);

            var stringResult = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(stringResult, "true");

        }

        [Test]
        public async Task Negative_Test_Update_Menu_Item_Updates_Menu_Item()
        {
            var newMenuItem = new MenuItem()
            {
                Id = 999,
                Description = "Updated Description",
                Name = "Updated Name",
                StockCount = 10
            };
            var expected = new ExceptionDto()
            {
                Message = "Internal Server Error.",
                StatusCode = 500
            };
            var content = JsonConvert.SerializeObject(newMenuItem);
            var stringContent = new StringContent(content, UnicodeEncoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"/1/menu-item", stringContent);

            var stringResult = await response.Content.ReadFromJsonAsync<ExceptionDto>();

            Assert.AreEqual(stringResult.Message, expected.Message);
        }
        //[Test]
    }
}
