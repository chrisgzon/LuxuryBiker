using LuxuryBiker.Api.Authentication;
using LuxuryBiker.Api.Middlewares;
using LuxuryBiker.Application.Common.Interfaces.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace LuxuryBiker.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddCors(configuration);
            services.AddControllers(options =>
            {
                options.AllowEmptyInputInBodyModelBinding = true;
            });

            services.AddEndpointsApiExplorer();
            
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Luxury Biker API V1",
                    Description = "API for enableds services to query in backend of application Luxury Biker",
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                        new string[] {}
                    }
                });
            });

            services.AddScoped<IUser, CurrentUser>();
            services.AddTransient<GlobalExceptionHandlingMiddleware>();
            return services;
        }

        private static IServiceCollection AddCors(this IServiceCollection services, IConfiguration configuration)
        {
            var originsSection = configuration.GetSection("PoliciesCors:ClientAngular:origins");
            string[] origins = originsSection.Get<string[]>();
            services.AddCors(options =>
            {
                options.AddPolicy(name: configuration["PoliciesCors:ClientAngular:name"]!,
                                  policy =>
                                  {
                                      policy.WithOrigins(origins!);
                                      policy.AllowAnyHeader();
                                  });
            });
            return services;
        }
    }
}
