using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace DotNet.Cookbook.Authentication
{
    public static class ActionContextExtensions
    {
        public static IEnumerable<T> GetAttributesOfType<T>(this ActionContext context)
        {
            return context?.ActionDescriptor?.FilterDescriptors?
                .Where(x => x.Filter is T)
                .Select(x => (T)x.Filter);
        }
    }
}
