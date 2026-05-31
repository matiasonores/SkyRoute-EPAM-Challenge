using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace SkyRoute.Application
{
    public static class ApplicationExtension
    {
        /// <param name="services">The DI container.</param>
        /// <param name="additionalAssemblies">
        /// Extra assemblies (e.g. the API project) whose AutoMapper profiles should be
        /// registered alongside the Application layer profiles.
        /// </param>
        public static IServiceCollection AddApplication(this IServiceCollection services, params Assembly[] additionalAssemblies)
        {
            var assemblies = new[] { Assembly.GetExecutingAssembly() }
                .Concat(additionalAssemblies)
                .ToArray();

            services.AddAutoMapper(assemblies);

            return services;
        }
    }
}

