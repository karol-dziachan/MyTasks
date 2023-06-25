using Microsoft.EntityFrameworkCore;

namespace MyTasks.Application.Common.Interfaces
{
    public interface IMyTasksDbContext
    {
        DbSet<Domain.Entities.Task> Tasks { get; set; }
        DbSet<Domain.Entities.Note> Notes { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
