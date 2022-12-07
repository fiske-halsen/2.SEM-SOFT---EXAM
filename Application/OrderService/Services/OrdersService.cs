using System.Diagnostics;
using Common.Dto;
using OrderService.Models;
using OrderService.Repository;

namespace OrderService.Services
{
    public interface IOrderService
    {
        public Task<Order> DenyOrder(int id);
        public Task<Order> AcceptOrder(int id);
        public Task<Order> CancelOrder(int id);
        public Task<bool> CreateOrder(CreateOrderDto createOrderDto);
    }

    public class OrdersService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersService(IOrderRepository orderRepository)
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
                    TotalPrice = createOrderDto.MenuItems.Sum(_ => _.Price),
                    MenuItems = createOrderDto.MenuItems.Select(_ => new OrderItem { ItemPrice = _.Price })
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

        public async Task<Order> CancelOrder(int id)
        {
            try
            {
                Order order = await _orderRepository.CancelOrder(id);
                return order;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        public async Task<Order> AcceptOrder(int id)
        {
            try
            {
                Order order = await _orderRepository.AcceptOrder(id);
                return order;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        public async Task<Order> DenyOrder(int id)
        {
            try
            {
                Order order = await _orderRepository.DenyOrder(id);
                return order;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }
    }
}