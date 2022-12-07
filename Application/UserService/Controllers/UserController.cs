using Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Services;

namespace Microservice1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// End point for creating new user
        /// </summary>
        /// <param Name="createUserDto"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<bool> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            return await _userService.CreateUser(createUserDto);
        }

        /// <summary>
        /// End point to update user balance
        /// </summary>
        /// <param Name="updateUserBalanceDto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch]
        public async Task<bool> UpdateUserBalance([FromBody] UpdateUserBalanceDto updateUserBalanceDto)
        {
            return await _userService.UpdateUserBalance(updateUserBalanceDto);
        }
    }
}