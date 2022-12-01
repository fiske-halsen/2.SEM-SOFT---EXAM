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
        public Task<bool> CreateUser(User user);
    }

    /// <summary>
    /// Layer for communicating with the userservice db
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly DbApplicationContext _applicationContext;

        public UserRepository(DbApplicationContext dbApplicationContext)
        {
            _applicationContext = dbApplicationContext;
        }

        /// <summary>
        /// Creates a user
        /// </summary>
        /// <param name="user"> The user entity</param>
        /// <returns></returns>
        public async Task<bool> CreateUser(User user)
        {
            try
            {
                _applicationContext.Users.Add(user);
                await _applicationContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<User> GetUserByEmail(string email)
        {
            return await _applicationContext.Users.Include(_ => _.Role)
                .FirstOrDefaultAsync(_ => _.Email == email);
        }

        /// <summary>
        /// Gets a user by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<User> GetUserById(int userId)
        {
            return await _applicationContext.Users.Include(_ => _.Role)
                .FirstOrDefaultAsync(_ => _.Id == userId);
        }

        /// <summary>
        /// Gets a user role by id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Role> GetUserRoleById(int userId)
        {
            var user = await _applicationContext.Users.Include(_ => _.Role)
                .FirstOrDefaultAsync(_ => _.Id == userId);

            return user.Role;
        }
    }
}