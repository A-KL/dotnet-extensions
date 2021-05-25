using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotNet.Cookbook.Configuration
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddConfiguration<TConfiguration>(this IServiceCollection services)
            where TConfiguration : class
        {
            return services
                .AddSingleton(x => x
                    .GetService<IConfiguration>()
                    .Get<TConfiguration>());
        }

        public static IServiceCollection AddConfiguration<TConfiguration>(this IServiceCollection services, string path)
            where TConfiguration : class
        {
            return services
                .AddSingleton(x => x
                    .GetService<IConfiguration>()
                    .GetSection(path)
                    .Get<TConfiguration>());
        }

        public static IServiceCollection AddConfiguration<TInterface, TConfiguration>(this IServiceCollection services, string path)
            where TInterface : class
            where TConfiguration : class, TInterface
        {
            return services
                .AddSingleton<TInterface, TConfiguration>(x => x
                    .GetService<IConfiguration>()
                    .GetSection(path)
                    .Get<TConfiguration>());
        }

        public static TConfiguration GetDefaults<TConfiguration>(this IConfiguration configuration, string path = default)
            where TConfiguration : class
        {
            return configuration
                    .GetSection(path ?? typeof(TConfiguration).Name)
                    .Get<TConfiguration>();
        }
    }
}
