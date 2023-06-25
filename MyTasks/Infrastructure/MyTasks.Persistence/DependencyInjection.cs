using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyTasks.Application.Common.Interfaces;
using MyTasks.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MyTasks.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MyTasksDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("MyTasksDb"), b => b.MigrationsAssembly("MyTasks.Api")));
            services.AddScoped<IMyTasksDbContext, MyTasksDbContext>();

            return services;
        }
    }
}
