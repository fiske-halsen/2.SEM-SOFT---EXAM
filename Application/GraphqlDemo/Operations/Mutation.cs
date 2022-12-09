using Common.Dto;
using GraphqlDemo.Services;
using System.Diagnostics;

namespace GraphqlDemo.Operations
{
    public class Mutation
    {
        private readonly IConfiguration _configuration;
        private readonly IUserServiceCommunicator _userServiceCommunicator;
        private readonly IOrderServiceCommunicator _orderServiceCommunicator;

        public Mutation(
            IConfiguration configuration,
            IUserServiceCommunicator userServiceCommunicator,
            IOrderServiceCommunicator orderServiceCommunicator)
        {
            _configuration = configuration;
            _userServiceCommunicator = userServiceCommunicator;
            _orderServiceCommunicator = orderServiceCommunicator;
        }

        #region OrderService

        /// <summary>
        /// Creates a new order and posts a ValidatePayment to kafka
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<bool> CreateOrder(CreateOrderDto dto)
        {
            try
            {
                await _orderServiceCommunicator.CreateOrder(dto);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Approves order and orchestrates events to both restaurant service and order service
        /// </summary>
        /// <param name="approveOrderDto"></param>
        /// <returns></returns>
        public async Task<bool> ApproveOrder(ApproveOrderDto approveOrderDto)
        {
            try
            {
                await _orderServiceCommunicator.ApproveOrder(approveOrderDto);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        #endregion

        #region UserService

        /// <summary>
        /// Creates a new user to the system; Sends a call to UserService
        /// </summary>
        /// <param Name="createUserDto"></param>
        /// <returns></returns>
        public async Task<bool> CreateUser(CreateUserDto createUserDto)
        {
            try
            {
                await _userServiceCommunicator.CreateUser(createUserDto);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Login for a user
        /// </summary>
        /// <param Name="loginUserDto"></param>
        /// <returns></returns>
        public async Task<TokenDto> Login(LoginUserDto loginUserDto)
        {
            try
            {
                return await _userServiceCommunicator.Login(loginUserDto);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateUserBalanceDto"></param>
        /// <returns></returns>
        public async Task<bool> AddCreditToUserBalance(UpdateUserBalanceDto updateUserBalanceDto)
        {
            try
            {
                return await _userServiceCommunicator.AddToUserBalance(updateUserBalanceDto);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }

        }

        #endregion
    }
}