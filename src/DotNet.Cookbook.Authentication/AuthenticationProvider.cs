using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

using DotNet.Cookbook.Authentication.Abstractions;

namespace DotNet.Cookbook.Authentication
{
    public class AuthenticationProvider : IAuthenticationProvider
    {
        private readonly string RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
        private readonly string EmailClaimType = "upn";

        private readonly IHttpContextAccessor _contextAccessor;

        public AuthenticationProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string CurrentUserEmail()
        {
            return _contextAccessor.HttpContext.User.Claims
                .Where(x => x.Type == EmailClaimType)
                .FirstOrDefault()
                ?.Value;
        }

        public IList<string> Groups()
        {
            return _contextAccessor.HttpContext.User.Claims
                .Where(x => x.Type == RoleClaimType)
                .Select(x => x.Value)
                .ToList();
        }
    }
}
