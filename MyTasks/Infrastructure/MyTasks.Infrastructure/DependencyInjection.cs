using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTasks.Application.Common.Interfaces;
using MyTasks.Infrastructure.Services;

namespace MyTasks.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IDateTime, DateTimeService>();
            return services;
        }
    }
}
