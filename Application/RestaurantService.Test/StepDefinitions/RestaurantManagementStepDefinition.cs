using System;
using System.Net.Http.Json;
using Common.Dto;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace RestaurantService.Test.StepDefinitions
{
    [Binding]
    public class RestaurantManagementStepDefinition
    {
        private readonly HttpClient _httpClient;
        private readonly ScenarioContext _scenarioContext;
        public RestaurantManagementStepDefinition(HttpClient httpClient, ScenarioContext scenarioContext)
        {
            _httpClient = httpClient;
            _scenarioContext = scenarioContext;
        }

        [When(@"restaurant owner creates menu item")]
        public async Task WhenRestaurantOwnerCreatesMenuItem()
        {
            var menuItemDTO = new MenuItemDTO{Id = 1, Description = "test", Name = "name_test", Price = 20, StockCount = 12};
            var createdMenuItem = new MenuItemDTO();

            var response = await _httpClient.PostAsJsonAsync("1/menu-item", menuItemDTO);
            var responseMenuItem = await response.Content.ReadFromJsonAsync<MenuItemDTO>();
            createdMenuItem = responseMenuItem;
            _scenarioContext.Add("CreatedMenuItem", createdMenuItem);

        }
        

        [Then(@"the menu item is created successfully")]
        public async Task ThenTheMenuItemIsCreatedSuccessfully()
        {
            var createdMenuItem = _scenarioContext.Get<MenuItemDTO>("CreatedMenuItem");
            var response = await _httpClient.GetFromJsonAsync<MenuItemDTO>($"$1/menu-item/{createdMenuItem.Id}");
            createdMenuItem.Should().BeEquivalentTo(response);
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
