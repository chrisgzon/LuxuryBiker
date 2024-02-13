using LuxuryBiker.Application.Common.Interfaces;
using LuxuryBiker.Application.Common.Interfaces.Services;
using LuxuryBiker.Domain.Constants;
using LuxuryBiker.Infrastructure.Persistence;
using LuxuryBiker.Infrastructure.Services.Authentication;
using LuxuryBiker.Infrastructure.Services.Authentication.JWT;
using LuxuryBiker.Infrastructure.Services.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LuxuryBiker.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthorization()
                .AddAuthentication(configuration)
                .AddPersistence(configuration);

            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LuxuryBikerDbContext>((sp, options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("LuxuryBiker")!);
            });

            services.AddScoped<ILuxuryBikerDbContext>(provider => provider.GetRequiredService<LuxuryBikerDbContext>());
            services.AddScoped<LuxuryBikerDbContextInitialiser>();

            return services;
        }

        private static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JWTSettings>(configuration.GetSection(JWTSettings.Section));

            services.AddTransient<IAuthenticationService<ApplicationUserDTO>, AuthenticationService>();

            services
                .ConfigureOptions<JwtBearerTokenValidationConfiguration>()
                .AddHttpContextAccessor()
                .AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

            return services;
        }

        private static IServiceCollection AddAuthorization(this IServiceCollection services)
        {
            services
                .AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<LuxuryBikerDbContext>()
                .AddSignInManager<SignInManager<ApplicationUser>>();

            services.AddTransient<IIdentityService<ApplicationUser>, IdentityService>();
            services.AddAuthorization(options =>
            options.AddPolicy(Policies.CanChangeStatusSales, policy => policy.RequireRole(Roles.Administrator)));

            return services;
        }
    }
}
