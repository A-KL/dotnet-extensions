using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNet.Cookbook.Authentication
{
    public class EnforceRoleAuthenticationAttribute : Attribute, IFilterMetadata
    {
        public IEnumerable<string> ValidRoles { get; private set; }

        /// <summary>
        /// Ensures that the user making the current <see cref="HttpRequest"/> has at least one of the required roles/>
        /// </summary>
        /// <param name="validRoles">Roles to validate against</param>
        public EnforceRoleAuthenticationAttribute(params string[] validRoles)
        {
            if (validRoles != null &&
                validRoles.Length > 0)
            {
                ValidRoles = validRoles;
            }
        }
    }
}
