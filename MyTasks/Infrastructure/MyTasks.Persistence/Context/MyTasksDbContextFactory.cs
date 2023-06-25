using Microsoft.EntityFrameworkCore;

namespace MyTasks.Persistence.Context
{
    internal class MyTasksDbContextFactory : DesignTimeDbContextFactoryBase<MyTasksDbContext>
    {
        protected override MyTasksDbContext CreateNewInstance(DbContextOptions<MyTasksDbContext> options)
        {
            return new MyTasksDbContext(options);
        }
    }
}
