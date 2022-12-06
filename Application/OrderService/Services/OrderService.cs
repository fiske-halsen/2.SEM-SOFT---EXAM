using System.Diagnostics;
using Common.Dto;
using OrderService.Models;
using OrderService.Repository;

namespace OrderService.Services
{
    public interface IOrderService
    {
        public Task<bool> CreateOrder(CreateOrderDto createOrderDto);
    }

    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<bool> CreateOrder(CreateOrderDto createOrderDto)
        {
            try
            {
                var order = new Order
                {
                    CustomerEmail = createOrderDto.CustomerEmail,
                    IsApproved = false,
                    IsActive = true,
                    RestaurantId = createOrderDto.RestaurantId,
                    TotalPrice = createOrderDto.MenuItems.Sum(_ => _.price),
                    MenuItems = createOrderDto.MenuItems.Select(_ => new OrderItem {Name = _.name, ItemPrice = _.price})
                        .ToList()
                };

                await _orderRepository.CreateOrder(order);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return false;
            }
        }
    }
}