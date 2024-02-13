using LuxuryBiker.Application.Common.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace LuxuryBiker.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblyContaining<ApplicationAssemblyReference>();
                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            });

            return services;
        }
    }
}