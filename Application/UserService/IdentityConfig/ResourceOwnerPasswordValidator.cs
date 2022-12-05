using IdentityServer4.Models;
using IdentityServer4.Validation;
using UserService.Repository;
using UserService.Services;

namespace UserService.IdentityConfig
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserService _userService;

        public ResourceOwnerPasswordValidator(IUserService userService)
        {
            _userService = userService;
        }

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var userName = context.UserName;
            var password = context.Password;

            try
            {
                var user = _userService.GetUserByEmail(userName).Result;
                bool isEqual = BCrypt.Net.BCrypt.Verify(password, user.Password);

                // Succes login
                if (user != null && isEqual)
                {
                    context.Result =
                        new GrantValidationResult(subject: user.Id.ToString(), authenticationMethod: "password");
                    return Task.FromResult(context.Result);
                }

                // Fail login
                context.Result =
                    new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Wrong username or password");
            }
            catch (Exception e)
            {
                throw e;
            }

            return Task.FromResult(context.Result);
        }
    }
}