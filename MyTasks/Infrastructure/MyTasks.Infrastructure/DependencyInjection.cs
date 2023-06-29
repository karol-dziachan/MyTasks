using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTasks.Application.Common.Interfaces;
using MyTasks.Infrastructure.Holders;
using MyTasks.Infrastructure.Services;

namespace MyTasks.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDateTime, DateTimeService>();

            var auth0Uri = configuration["Auth0:Domain"];
            var clientId = configuration["Auth0:ClientId"];
            var clientSecret = configuration["Auth0:ClientSecret"];

            if (string.IsNullOrEmpty(auth0Uri) || string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException(nameof(auth0Uri));
            }

            services.AddTransient<IAuthInformationsHolder, AuthInformationsHolder>();
            services.AddScoped<IAuthService>(provider => new AuthService(auth0Uri, clientId, clientSecret));

            return services;
        }
    }
}
