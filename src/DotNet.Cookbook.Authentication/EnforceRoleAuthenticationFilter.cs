using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using DotNet.Cookbook.Authentication.Abstractions;

namespace DotNet.Cookbook.Authentication
{
    public class EnforceRoleAuthenticationFilter : IAsyncAuthorizationFilter
    {
        protected readonly IAuthenticationProvider _provider;

        public EnforceRoleAuthenticationFilter(IAuthenticationProvider provider)
        {
            _provider = provider;
        }

        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var roleAuthenticationAttribute = context
                .GetAttributesOfType<EnforceRoleAuthenticationAttribute>()
                .FirstOrDefault();

            if (roleAuthenticationAttribute?.ValidRoles != null)
            {
                if (!roleAuthenticationAttribute.ValidRoles.Any(x => _provider.HasRole(x)))
                {
                    context.Result = new ForbidResult();
                }
            }

            return Task.CompletedTask;
        }
    }
}
