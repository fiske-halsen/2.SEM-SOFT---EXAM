using Common.Dto;
using Common.Enums;
using Common.ErrorModels;
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
    }

    public class UsersService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UsersService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

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
                RoleId = (int) Enums.RoleTypes.Customer,
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

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);

            if (user == null)
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest, "User does not exist");
            }

            return user;
        }

        public async Task<User> GetUserById(int userId)
        {
            var user = await _userRepository.GetUserById(userId);

            if (user == null)
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest, "User does not exist");
            }

            return user;
        }

        public async Task<Role> GetUserRoleById(int userId)
        {
            var user = await _userRepository.GetUserRoleById(userId);

            if (user == null)
            {
                throw new HttpStatusException(StatusCodes.Status400BadRequest, "User does not exist");
            }

            return user;
        }
    }
}