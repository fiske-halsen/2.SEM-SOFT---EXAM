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

        public async Task<int> TimeToDelivery(int id)
        {
            try
            {
                var time = await _orderRepository.TimeToDelivery(id);
                return time;
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