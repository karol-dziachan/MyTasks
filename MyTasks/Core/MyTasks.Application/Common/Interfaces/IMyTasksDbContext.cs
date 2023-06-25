using Microsoft.EntityFrameworkCore;
using MyTasks.Domain.Entities;

namespace MyTasks.Application.Common.Interfaces
{
    public interface IMyTasksDbContext
    {
        DbSet<Domain.Entities.Task> Tasks { get; set; }
        DbSet<Note> Notes { get; set; }
        DbSet<UsersTasks> UsersTasks { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
