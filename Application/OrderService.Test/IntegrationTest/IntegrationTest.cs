using Bogus;
using OrderService.Models;
using OrderService.Test.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Test.IntegrationTest
{
    [TestFixture]
    public class IntegrationTest
    {
        private IntegrationTestWebFactory<Program> _factory;

        [OneTimeSetUp]
        public async Task Setup()
        {
            _factory = new IntegrationTestWebFactory<Program>();
            await _factory.InitializeAsync();
        }

        [TearDown]
        public async Task CleanUp()
        {
            await _factory.DisposeAsync();
        }

        //[Test]
        public async Task CreateOrderIntegrationTest()
        {
            //Arrange
            var order = new Order
            {
            };

            //Act

            //Assert
        }
    }
}
