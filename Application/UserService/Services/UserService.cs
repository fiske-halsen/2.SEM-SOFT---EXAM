using UserService.Models;

namespace UserService.Services
{
    public interface IUserService
    {
        public Task<User> GetUserById(int userId);
        public Task<User> GetUserByEmail(string email);
        public Task<Role> GetUserRoleById(int userId);
    }

    public class UserService : IUserService
    {
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
