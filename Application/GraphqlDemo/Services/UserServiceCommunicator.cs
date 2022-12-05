using Common.Dto;
using Newtonsoft.Json;

namespace GraphqlDemo.Services
{
    public interface IUserServiceCommunicator
    {
        public Task<bool> CreateUser(CreateUserDto createUserDto);
        public Task<TokenDto> Login(LoginUserDto loginUserDto);
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
        }

        public async Task<bool> CreateUser(CreateUserDto createUserDto)
        {
            var serializedUser = JsonConvert.SerializeObject(createUserDto);
            return await _apiService.Post(_userServiceUrl + "/api/user", serializedUser, null);
        }

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