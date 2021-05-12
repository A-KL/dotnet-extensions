using DotNet.Cookbook.Authentication.Abstractions;

namespace DotNet.Cookbook.Authentication
{
    public static class Extensions
    {
        public static bool HasRole(this IAuthenticationProvider provider, string role)
        {
            return provider
                .Groups()
                .Contains(role);
        }

        public static bool HasRole<T>(this IAuthenticationProvider provider, T role)
        {
            return provider
                .HasRole(role.ToString());
        }
    }
}
