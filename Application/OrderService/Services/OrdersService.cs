using System.Diagnostics;
using Common.Dto;
using Common.ErrorModels;
using OrderService.Models;
using OrderService.Repository;

namespace OrderService.Services
{
    public interface IOrderService
    {
        public Task<bool> AcceptOrder(int orderId);
        public Task<bool> DeleteOrder(int orderId);
        public Task<bool> CreateOrder(CreateOrderDto createOrderDto);
        public Task<List<Order>> GetAllOrdersForRestaurant(bool isApproved, int restaurantId);
        public Task<List<Order>> GetAllOrdersForRestaurant(int restaurantId);
        public Task<List<Order>> GetAllOrdersForUser(string userEmail);
        public Task<bool> UpdateOrderSetInActive(int orderId);
    }

    public class OrdersService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Creates a new order
        /// </summary>
        /// <param name="createOrderDto"></param>
        /// <returns></returns>
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
                    TotalPrice = createOrderDto.OrderTotal,
                    CardType = createOrderDto.CardType,
                    PaymentType = createOrderDto.PaymentType,
                    MenuItems = createOrderDto.MenuItems
                        .Select(_ => new OrderItem {MenuItemId = _.Id, ItemPrice = _.Price})
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

        /// <summary>
        /// gets all orders for restaurant depending on isApproved
        /// </summary>
        /// <param name="isApproved"></param>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        public async Task<List<Order>> GetAllOrdersForRestaurant(bool isApproved, int restaurantId)
        {
            return await _orderRepository.GetAllOrdersForRestaurant(isApproved, restaurantId);
        }

        /// <summary>
        /// Gets all orders for restaurant
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        public async Task<List<Order>> GetAllOrdersForRestaurant(int restaurantId)
        {
            return await _orderRepository.GetAllOrdersForRestaurant(restaurantId);
        }

        /// <summary>
        /// Deletes a order by order id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<bool> DeleteOrder(int orderId)
        {
            try
            {
                var order = await _orderRepository.GetOrderById(orderId);

                if (order == null)
                {
                    throw new HttpStatusException(StatusCodes.Status400BadRequest, "Given order does not exist");
                }

                await _orderRepository.DeleteOrder(order);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Accepts a given order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<bool> AcceptOrder(int orderId)
        {
            try
            {
                var order = await _orderRepository.GetOrderById(orderId);

                if (order == null)
                {
                    throw new HttpStatusException(StatusCodes.Status400BadRequest, "Given order does not exist");
                }

                await _orderRepository.AcceptOrder(orderId);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Gets orders for a specific user
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<List<Order>> GetAllOrdersForUser(string userEmail)
        {
            var ordersForUser = await _orderRepository.GetOrdersForUser(userEmail);

            if (!ordersForUser.Any())
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest,
                    $"No orders for the given user {userEmail}");
            }

            return ordersForUser;
        }

        /// <summary>
        /// Updates a order sets the order to inactive..
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<bool> UpdateOrderSetInActive(int orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId);

            if (order == null)
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest, "Given order does not exist");
            }

            return await _orderRepository.UpdateOrderSetInActive(order);
        }
    }
}