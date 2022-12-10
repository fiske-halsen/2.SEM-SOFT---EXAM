﻿using Bogus;
using Common.Dto;
using GraphqlDemo.Services;

namespace GraphqlDemo.Operations
{
    public class Query
    {
        private readonly IOrderServiceCommunicator _orderServiceCommunicator;
        private readonly IReviewServiceCommunicator _reviewServiceCommunicator;
        private readonly IDeliveryServiceCommunicator _deliveryServiceCommunicator;

        public Query(IOrderServiceCommunicator orderServiceCommunicator,
            IReviewServiceCommunicator reviewServiceCommunicator,
            IDeliveryServiceCommunicator deliveryServiceCommunicator)
        {
            _orderServiceCommunicator = orderServiceCommunicator;
            _reviewServiceCommunicator = reviewServiceCommunicator;
            _deliveryServiceCommunicator = deliveryServiceCommunicator;
        }


        #region OrderService

        /// <summary>
        /// Gets all orders by a specific restaurant and user
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        public async Task<IEnumerable<OrderDto>> GetAllOrdersForRestaurantByUser(int restaurantId, string userEmail)
        {
            return await _orderServiceCommunicator.GetAllOrdersForRestaurantsByUser(restaurantId, userEmail);
        }

        /// <summary>
        /// Gets all orders for a specific restaurant
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<OrderDto>> GetOrdersForRestaurant(int restaurantId)
        {
            return await _orderServiceCommunicator.GetOrdersForRestaurants(restaurantId);
        }

        /// <summary>
        /// gets all orders for a specific restaurant, where you can get Approved/nonapproved orders depending on isApproved
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <param name="isApproved"></param>
        /// <returns></returns>
        public async Task<IEnumerable<OrderDto>> GetOrdersForRestaurantWithIsApproved(int restaurantId, bool isApproved)
        {
            return await _orderServiceCommunicator.GetOrdersForRestaurants(restaurantId, isApproved);
        }

        public List<MenuDTO> GetOrder()
        {
            //Faker<MenuItem> menuItemFaker = new Faker<MenuItem>()
            //    .StrictMode(true)
            //    .RuleFor(x => x.Id, x => x.Random.Int())
            //    .RuleFor(x => x.Price, x => x.Random.Int())
            //    .RuleFor(x => x.Name, x => x.Name.FirstName());

            Faker<MenuDTO> orderFaker = new Faker<MenuDTO>()
                .RuleFor(x => x.RestaurantName, x => x.Name.FirstName());

            return orderFaker.Generate(10);
            return null;
        }

        #endregion

        #region ReviewService

        public async Task<IEnumerable<CreateReviewDto>> GetReviewsByDeliveryUserId(int deliveryUserId)
        {
            return await _reviewServiceCommunicator.GetReviewsByDeliveryUserId(deliveryUserId);
        }

        public async Task<IEnumerable<CreateReviewDto>> GetReviewsByRestaurantId(int restaurantId)
        {
            return await _reviewServiceCommunicator.GetReviewsByRestaurantId(restaurantId);
        }

        public async Task<IEnumerable<CreateReviewDto>> GetReviewsByUserId(int userId)
        {
            return await _reviewServiceCommunicator.GetReviewsByUserId(userId);
        }

        #endregion


        #region DeliveryService

        /// <summary>
        /// Gets deliveries by order id
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<DeliveryDto> GetDeliveriesByOrderId(int orderId)
        {
            return await _deliveryServiceCommunicator.GetDeliveryByOrderId(orderId);
        }

        /// <summary>
        /// Gets deliveries by Delivery person Id
        /// </summary>
        /// <param name="deliveryPersonId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DeliveryDto>> GetDeliveriesByDeliveryPersonId(int deliveryPersonId)
        {
            return await _deliveryServiceCommunicator.GetDeliveriesByDeliveryPersonId(deliveryPersonId);
        }

        /// <summary>
        /// gets deliveries by user email
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DeliveryDto>> GetDeliveriesByUserEmail(string userEmail)
        {
            return await _deliveryServiceCommunicator.GetDeliveryByUserEmail(userEmail);
        }

        #endregion
    }
}