using DeliveryService.Repository;
using DeliveryService.Services;
using Moq;

namespace DeliveryService.Test
{
    public class DeliveryRepositoryUnittest
    {

        private Mock<IDeliveryRepository> _deliveryRepositoryMock;
        private IDeliverySerivce _deliverySerivce;

        [SetUp]
        public void Setup()
        {
            _deliveryRepositoryMock = new Mock<IDeliveryRepository>();
            _deliverySerivce = new DeliveryService.Services.DeliveryService(_deliveryRepositoryMock.Object)
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}