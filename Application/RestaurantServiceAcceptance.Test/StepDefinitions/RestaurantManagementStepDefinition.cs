using System;
using System.Net.Http.Json;
using Common.Dto;
using Microsoft.AspNetCore.Mvc.Testing;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace RestaurantService.Test.StepDefinitions
{
    [Binding]
    public class RestaurantManagementStepDefinition
    {
        private HttpClient _httpClient;
        private readonly ScenarioContext _scenarioContext;
        private WebApplicationFactory<Program> _webApplicationFactory;
        public RestaurantManagementStepDefinition(HttpClient httpClient, ScenarioContext scenarioContext, WebApplicationFactory<Program> webApplicationFactory)
        {
            _httpClient = httpClient;
            _scenarioContext = scenarioContext;
            _webApplicationFactory = webApplicationFactory;
            
        }

        [When(@"restaurant owner creates menu item")]
        public async Task WhenRestaurantOwnerCreatesMenuItem()
        {
            _webApplicationFactory = new WebApplicationFactory<Program>();
            _httpClient = _webApplicationFactory.CreateClient();

            var menuItemDTO = new MenuItemDTO{Id = 1, Description = "test", Name = "name_test", Price = 20, StockCount = 12};

            var response = await _httpClient.PostAsJsonAsync("1/menu-item", menuItemDTO);
            var responseMenuItem = await response.Content.ReadAsStringAsync();
            _scenarioContext.Add("ResponseMenuItem", responseMenuItem);

        }
        

        [Then(@"the menu item is created successfully")]
        public async Task ThenTheMenuItemIsCreatedSuccessfully()
        {
            var createdMenuItem = _scenarioContext.Get<string>("ResponseMenuItem");
           // var response = await _httpClient.GetFromJsonAsync<MenuItemDTO>($"$1/menu-item/{createdMenuItem.Id}");
            createdMenuItem.Should().BeEquivalentTo("true");
        }

        [When(@"restaurant owner deletes menu item")]
        public void WhenRestaurantOwnerDeletesMenuItem()
        {
            throw new PendingStepException();
        }

        [Then(@"the menu item is deleted successfully")]
        public void ThenTheMenuItemIsDeletedSuccessfully()
        {
            throw new PendingStepException();
        }

        [When(@"restaurant owner updates menu item")]
        public void WhenRestaurantOwnerUpdatesMenuItem()
        {
            throw new PendingStepException();
        }

        [Then(@"the menu item is updated successfully")]
        public void ThenTheMenuItemIsUpdatedSuccessfully()
        {
            throw new PendingStepException();
        }
    }
}
