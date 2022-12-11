using Microsoft.AspNetCore.Authorization;

namespace GraphqlDemo.Authorization
{
    public class RoleTypeRequirement : IAuthorizationRequirement
    {
        public string Issuer { get; }
        public int RoleId { get; set; }

        public RoleTypeRequirement(string issuer, int roleId)
        {
            Issuer = issuer;
            RoleId = roleId;
        }
    }
}