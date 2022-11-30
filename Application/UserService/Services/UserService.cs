using Common.Dto;
using UserService.Models;
using UserService.Repository;

namespace UserService.Services
{
    public interface IUserService
    {
        public Task<User> GetUserById(int userId);
        public Task<User> GetUserByEmail(string email);
        public Task<Role> GetUserRoleById(int userId);
        public Task<GenericResponse> CreateUser(CreateUserDto createUserDto);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<GenericResponse> CreateUser(CreateUserDto createUserDto)
        {
            throw new NotImplementedException();
        }

        //public async Task<GenericResponse> CreateUser(CreateUserDto createUserDto)
        //{
        //    if ((await _userRepository.GetUserByEmail(createUserDto.Email)) != null)
        //    {

        //    }

        //}

        public Task<User> GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserById(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<Role> GetUserRoleById(int userId)
        {
            throw new NotImplementedException();
        }


    }
}
