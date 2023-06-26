using MyTasks.Application.Common.Interfaces;

namespace MyTasks.Application.Abstraction
{
    public class BaseRequestHandler
    {
        protected readonly IMyTasksDbContext _context;

        public BaseRequestHandler(IMyTasksDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
    }
}
