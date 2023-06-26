using MediatR;
using MyTasks.Application.Abstraction;
using MyTasks.Application.Common.Interfaces;


namespace MyTasks.Application.Features.Tasks.Queries.GetTasks
{
    public class GetTasksQueryHandler : BaseRequestHandler,  IRequestHandler<GetTasksQuery, GetTasksVm>
    {
        public GetTasksQueryHandler(IMyTasksDbContext context) : base(context)
        {

        }

        public Task<GetTasksVm> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = _context.Tasks;

            if (!tasks.Any()) return default;

            var tasksVm = new GetTasksVm();

            tasks.ToList().ForEach(task =>
            {
                var adaptTask = new GetTaskDto()
                {
                    Id = task.Id,
                    Title = task.Title,
                    Content = task.Content,
                    StartDateTime = task.StartDateTime,
                    EndDateTime = task.EndDateTime,
                    Duration = task.Duration,
                    OwnerName = task.OwnerName,
                };
                tasksVm.TaskDtos.Add(adaptTask);
            });

            return Task.FromResult(tasksVm);
        }
    }
}
