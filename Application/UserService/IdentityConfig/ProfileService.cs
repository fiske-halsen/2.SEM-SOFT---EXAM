using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Security.Claims;
using UserService.Services;

namespace UserService.IdentityConfig
{
    public class ProfileService : IProfileService
    {
        private readonly IUserService _userService;

        public ProfileService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                var subject = context.Subject.Claims.ToList().Find(s => s.Type == "sub").Value;
                var userId = Int32.Parse(subject);

                var user = await _userService.GetUserById(userId);

                if (subject == null)
                {
                    return;
                }

                var claims = new List<Claim>
                {
                    new Claim("RoleId", user.RoleId.ToString()),
                    new Claim("Email", user.Email),
                    Helpers.GetRoleTypeClaim(user.Role.RoleType) // Used for claim based authorization
                };

                context.IssuedClaims = claims;
            }
            catch (Exception e)
            {
                return;
            }
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            return Task.FromResult(0);
        }
    }
}