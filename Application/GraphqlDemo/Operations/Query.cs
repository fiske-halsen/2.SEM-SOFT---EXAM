using Common.Dto;
using GraphQL;
using GraphqlDemo.Services;

namespace GraphqlDemo.Operations
{
    public class Query
    {
        private readonly IOrderServiceCommunicator _orderServiceCommunicator;
        private readonly IReviewServiceCommunicator _reviewServiceCommunicator;
        private readonly IDeliveryServiceCommunicator _deliveryServiceCommunicator;
        private readonly IRestaurantServiceCommunicator _restaurantServiceCommunicator;

        public Query(IOrderServiceCommunicator orderServiceCommunicator,
            IReviewServiceCommunicator reviewServiceCommunicator,
            IDeliveryServiceCommunicator deliveryServiceCommunicator,
            IRestaurantServiceCommunicator restaurantServiceCommunicator)
        {
            _orderServiceCommunicator = orderServiceCommunicator;
            _reviewServiceCommunicator = reviewServiceCommunicator;
            _deliveryServiceCommunicator = deliveryServiceCommunicator;
            _restaurantServiceCommunicator = restaurantServiceCommunicator;
        }

        #region OrderService

        /// <summary>
        /// Gets all orders by a specific restaurant and user
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        [Authorize("Customer")]
        public async Task<IEnumerable<OrderDto>> GetAllOrdersForUser(string userEmail)
        {
            return await _orderServiceCommunicator.GetAllOrdersForRestaurantsByUser( userEmail);
        }

        /// <summary>
        /// Gets all orders for a specific restaurant
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        [Authorize("RestaurantOwner")]
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
        [Authorize("RestaurantOwner")]
        public async Task<IEnumerable<OrderDto>> GetOrdersForRestaurantWithIsApproved(int restaurantId, bool isApproved)
        {
            return await _orderServiceCommunicator.GetOrdersForRestaurants(restaurantId, isApproved);
        }

        #endregion

        #region ReviewService
        /// <summary>
        /// Creates a review
        /// </summary>
        /// <param name="deliveryUserId"></param>
        /// <returns></returns>
        [Authorize("Delivery")]
        public async Task<IEnumerable<CreateReviewDto>> GetReviewsByDeliveryUserId(int deliveryUserId)
        {
            return await _reviewServiceCommunicator.GetReviewsByDeliveryUserId(deliveryUserId);
        }
        /// <summary>
        /// Gets a review by restaurant Id
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        [Authorize("RestaurantOwner")]
        public async Task<IEnumerable<CreateReviewDto>> GetReviewsByRestaurantId(int restaurantId)
        {
            return await _reviewServiceCommunicator.GetReviewsByRestaurantId(restaurantId);
        }
        /// <summary>
        /// Gets reviews for a specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize("Customer")]
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
        [Authorize("Delivery")]
        public async Task<DeliveryDto> GetDeliveryByOrderId(int orderId)
        {
            return await _deliveryServiceCommunicator.GetDeliveryByOrderId(orderId);
        }

        /// <summary>
        /// Gets deliveries by Delivery person Id
        /// </summary>
        /// <param name="deliveryPersonId"></param>
        /// <returns></returns>
        [Authorize("Delivery")]
        public async Task<IEnumerable<DeliveryDto>> GetDeliveriesByDeliveryPersonId(int deliveryPersonId)
        {
            return await _deliveryServiceCommunicator.GetDeliveriesByDeliveryPersonId(deliveryPersonId);
        }

        /// <summary>
        /// gets deliveries by user email
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        [Authorize("Customer")]
        public async Task<IEnumerable<DeliveryDto>> GetDeliveriesByUserEmail(string userEmail)
        {
            return await _deliveryServiceCommunicator.GetDeliveryByUserEmail(userEmail);
        }

        #endregion

        #region RestaurantService
        /// <summary>
        /// gets a specific restaurant menu
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <returns></returns>
        public async Task<MenuDTO> GetRestaurantMenu(int restaurantId)
        {
            return await _restaurantServiceCommunicator.GetRestaurantMenu(restaurantId);
        }
        /// <summary>
        /// Gets all restaurants
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "RestaurantOwner")]
        public async Task<IEnumerable<RestaurantDTO>> GetAllRestaurants()
        {
            return await _restaurantServiceCommunicator.GetAllRestaurants();
        }

        /// <summary>
        /// Gets a menu item for a specific restaurant
        /// </summary>
        /// <param name="restaurantId"></param>
        /// <param name="menuItemId"></param>
        /// <returns></returns>
        public async Task<MenuItemDTO> GetMenuItem(int restaurantId, int menuItemId)
        {
            return await _restaurantServiceCommunicator.GetMenuItem(restaurantId, menuItemId);
        }

        #endregion
    }
}