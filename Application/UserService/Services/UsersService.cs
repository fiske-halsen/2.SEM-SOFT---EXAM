using Common.Dto;
using Common.Enums;
using Common.ErrorModels;
using System.Diagnostics;
using UserService.Models;
using UserService.Repository;
using Address = UserService.Models.Address;

namespace UserService.Services
{
    public interface IUserService
    {
        public Task<User> GetUserById(int userId);
        public Task<User> GetUserByEmail(string email);
        public Task<Role> GetUserRoleById(int userId);
        public Task<bool> CreateUser(CreateUserDto createUserDto);
        public Task<bool> CheckIfUserBalanceHasEnoughCreditForOrder(CreateOrderDto createOrderDto);
        public Task<bool> UpdateUserBalanceForOrder(CreateOrderDto createOrderDto);
        public Task<bool> AddToUserBalance(UpdateUserBalanceDto updateUserBalanceDto);
    }

    /// <summary>
    /// User service contains the business logic
    /// </summary>
    public class UsersService : IUserService
    {
        private readonly IUserRepository _userRepository;
        
        public UsersService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Adds to a existing user balance
        /// </summary>
        /// <param name="updateUserBalanceDto"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<bool> AddToUserBalance(UpdateUserBalanceDto updateUserBalanceDto)
        {
            try
            {
                var user = await _userRepository.GetUserByEmail(updateUserBalanceDto.Email);

                if (user == null)
                {
                    throw new HttpStatusException(StatusCodes.Status400BadRequest, $"User does not exist");
                }

                await _userRepository.UpdateUserBalance(user, user.Balance + updateUserBalanceDto.Balance);

                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Checks if a given user has enough user credit to perform a order
        /// </summary>
        /// <param Name="createOrderDto"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<bool> CheckIfUserBalanceHasEnoughCreditForOrder(CreateOrderDto createOrderDto)
        {
            var user = await _userRepository.GetUserByEmail(createOrderDto.CustomerEmail);

            if (user == null)
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest, "User does not exist");
            }

            return user.Balance >= createOrderDto.OrderTotal;
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param Name="createUserDto"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<bool> CreateUser(CreateUserDto createUserDto)
        {
            if ((await _userRepository.GetUserByEmail(createUserDto.Email)) != null)
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest, "User email already exists");
            }

            if (!createUserDto.Password.Equals(createUserDto.PasswordRepeated))
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest, "Passwords does not match");
            }

            var user = new User
            {
                Email = createUserDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password),
                FirstName = createUserDto.FirstName,
                RoleId = (int) RoleTypes.Customer,
                Address = new Address
                {
                    StreetName = createUserDto.StreetName,
                    CityInfo = new CityInfo
                    {
                        City = createUserDto.City,
                        ZipCode = createUserDto
                            .ZipCode // This should be changed to find the actual zip, instead of creating a new one..
                    }
                }
            };

            await _userRepository.CreateUser(user);

            return true;
        }

        /// <summary>
        /// Gets a users balance by id
        /// </summary>
        /// <param Name="userId"></param>
        /// <returns></returns>
        public async Task<double> GetUserBalanceById(int userId)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null)
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest, $"User does not exist");
            }

            return user.Balance;
        }

        /// <summary>
        /// Gets a user by a email
        /// </summary>
        /// <param Name="email"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);

            if (user == null)
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest, $"User does not exist");
            }

            return user;
        }

        /// <summary>
        /// Gets a user by a given Id
        /// </summary>
        /// <param Name="userId"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<User> GetUserById(int userId)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null)
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest, $"User does not exist");
            }

            return user;
        }

        /// <summary>
        /// Gets a user by role
        /// </summary>
        /// <param Name="userId"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<Role> GetUserRoleById(int userId)
        {
            var user = await _userRepository.GetUserRoleById(userId);

            if (user == null)
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest, $"User does not exist");
            }

            return user;
        }

        /// <summary>
        /// Updates user balance
        /// </summary>
        /// <param Name="updateUserBalanceDto"></param>
        /// <returns></returns>
        /// <exception cref="HttpStatusException"></exception>
        public async Task<bool> UpdateUserBalanceForOrder(CreateOrderDto createOrderDto)
        {
            var user = await _userRepository.GetUserByEmail(createOrderDto.CustomerEmail);

            if (user == null)
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest,
                    $"User with the given id does not exist");
            }

            return await _userRepository.UpdateUserBalance(user, user.Balance - createOrderDto.OrderTotal);
        }
    }
}