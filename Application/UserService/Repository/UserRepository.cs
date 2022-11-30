using Microsoft.EntityFrameworkCore;
using UserService.Context;
using UserService.Models;

namespace UserService.Repository
{
    public interface IUserRepository
    {
        public Task<User> GetUserById(int userId);
        public Task<User> GetUserByEmail(string email);
        public Task<Role> GetUserRoleById(int userId);
    }

    public class UserRepository : IUserRepository
    {
        private readonly DbApplicationContext _applicationContext;

        public UserRepository(DbApplicationContext dbApplicationContext)
        {
            _applicationContext = dbApplicationContext;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _applicationContext.Users.
                 Include(_ => _.Role)
                .FirstOrDefaultAsync(_ => _.Email == email);
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _applicationContext.Users.
                 Include(_ => _.Role)
                .FirstOrDefaultAsync(_ => _.Id == userId);
        }

        public async Task<Role> GetUserRoleById(int userId)
        {
            var user = await _applicationContext.Users.
                Include(_ => _.Role)
               .FirstOrDefaultAsync(_ => _.Id == userId);

            return user.Role;
        }
    }
}
