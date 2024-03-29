﻿using Common.Dto;
using Newtonsoft.Json;

namespace GraphqlDemo.Services
{
    public interface IUserServiceCommunicator
    {
        public Task<bool> CreateUser(CreateUserDto createUserDto);
        public Task<TokenDto> Login(LoginUserDto loginUserDto);
        public Task<bool> AddToUserBalance(UpdateUserBalanceDto updateUserBalanceDto);
    }

    public class UserServiceCommunicator : IUserServiceCommunicator
    {
        private readonly IApiService _apiService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HelperService _helperService;
        private readonly ApplicationCredentials _applicationCredentials;
        private string _userServiceUrl;

        public UserServiceCommunicator(
            IApiService apiService,
            ITokenService tokenService,
            IConfiguration configuration, 
            IHttpContextAccessor httpContextAccessor,
            HelperService helperService)
            
        {
            _apiService = apiService;
            _tokenService = tokenService;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _helperService = helperService;
            _userServiceUrl = configuration["IdentityServer:Host"];
            _applicationCredentials = _helperService.GetMicroServiceApplicationCredentials(HelperService.ClientType.UserService);
        }

        /// <summary>
        /// Sends a call to user service to add to a users balance credit
        /// </summary>
        /// <param name="updateUserBalanceDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> AddToUserBalance(UpdateUserBalanceDto updateUserBalanceDto)
        {
            var serializedUserBalanceDto = JsonConvert.SerializeObject(updateUserBalanceDto);
            return await _apiService.Patch(_userServiceUrl + "/api/user", serializedUserBalanceDto, _applicationCredentials);
        }

        /// <summary>
        /// Sends a call to user service to create a new user
        /// </summary>
        /// <param name="createUserDto"></param>
        /// <returns></returns>
        public async Task<bool> CreateUser(CreateUserDto createUserDto)
        {
            var serializedUser = JsonConvert.SerializeObject(createUserDto);
            return await _apiService.Post(_userServiceUrl + "/api/user", serializedUser, null);
        }

        /// <summary>
        /// Sends a http call to the identity server to get a token
        /// </summary>
        /// <param name="loginUserDto"></param>
        /// <returns></returns>
        public async Task<TokenDto> Login(LoginUserDto loginUserDto)
        {
            var token = await _tokenService.RequestTokenForUser(loginUserDto.Username, loginUserDto.Password);
            return new TokenDto
            {
                AccessToken = token.TokenResponse.AccessToken,
                ExpiresIn = token.TokenResponse.ExpiresIn,
            };
        }
    }
}