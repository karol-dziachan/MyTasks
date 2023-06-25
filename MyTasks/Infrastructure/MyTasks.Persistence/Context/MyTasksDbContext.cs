using Microsoft.EntityFrameworkCore;
using MyTasks.Application.Common.Interfaces;
using MyTasks.Domain.Common;
using MyTasks.Domain.Entities;
using System.Reflection;

namespace MyTasks.Persistence.Context
{
    public class MyTasksDbContext : DbContext, IMyTasksDbContext
    {
        private readonly IDateTime _dateTime;

        public DbSet<Domain.Entities.Task> Tasks { get; set; }
        public DbSet<Note> Notes { get; set; }

        public MyTasksDbContext(DbContextOptions<MyTasksDbContext> options, IDateTime dateTime) : base(options)
        {
            _dateTime = dateTime;
        }

        public MyTasksDbContext(DbContextOptions<MyTasksDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = string.Empty;
                        entry.Entity.Created = _dateTime.Now;
                        entry.Entity.StatusId = 1;
                        break;
                    case EntityState.Modified:
                        entry.Entity.CreatedBy = string.Empty;
                        entry.Entity.Created = _dateTime.Now;
                        entry.Entity.ModifiedBy = String.Empty;
                        entry.Entity.Modified = _dateTime.Now;
                        entry.Entity.StatusId = 1;
                        break;
                    case EntityState.Deleted:
                        entry.Entity.ModifiedBy = String.Empty;
                        entry.Entity.Modified = _dateTime.Now;
                        entry.Entity.Inactivated = _dateTime.Now;
                        entry.Entity.InactivatedBy = String.Empty;
                        entry.Entity.StatusId = 0;
                        entry.State = EntityState.Modified;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
