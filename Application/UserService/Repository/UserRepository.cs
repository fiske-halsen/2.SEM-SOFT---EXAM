using System.Diagnostics;
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
        public Task<bool> UpdateUserBalance(User user, double newBalance);
    }

    /// <summary>
    /// Layer for communicating with the userservice db
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _applicationContext;

        public UserRepository(UserDbContext dbApplicationContext)
        {
            _applicationContext = dbApplicationContext;
        }

        /// <summary>
        /// Creates a user
        /// </summary>
        /// <param Name="user"> The user entity</param>
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
        /// <param Name="email"></param>
        /// <returns></returns>
        public async Task<User> GetUserByEmail(string email)
        {
            try
            {
                var user = await _applicationContext.Users.Include(_ => _.Role)
                    .FirstOrDefaultAsync(_ => _.Email == email);

                return user;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return null;
        }

        /// <summary>
        /// Gets a user by id
        /// </summary>
        /// <param Name="userId"></param>
        /// <returns></returns>
        public async Task<User> GetUserById(int userId)
        {
            try
            {
                var user = await _applicationContext.Users.Include(_ => _.Role)
                    .FirstOrDefaultAsync(_ => _.Id == userId);

                return user;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return null;
        }

        /// <summary>
        /// Gets a user role by id
        /// </summary>
        /// <param Name="userId"></param>
        /// <returns></returns>
        public async Task<Role> GetUserRoleById(int userId)
        {
            try
            {
                var user = await _applicationContext.Users.Include(_ => _.Role)
                    .FirstOrDefaultAsync(_ => _.Id == userId);

                return user.Role;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            return null;
        }

        /// <summary>
        /// Updates a user balance 
        /// </summary>
        /// <param Name="user"> The given user that needs a update</param>
        /// <param Name="newBalance">The new balance for the given user</param>
        /// <returns></returns>
        public async Task<bool> UpdateUserBalance(User user, double newBalance)
        {
            try
            {
                user.Balance = newBalance;
                await _applicationContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
    }
}