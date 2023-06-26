using MediatR;
using MyTasks.Application.Abstraction;
using MyTasks.Application.Common.Interfaces;
using MyTasks.Infrastructure.Services;

namespace MyTasks.Application.Features.Tasks.Queries.GetTasks
{
    public class GetTasksQueryHandler : BaseRequestHandler,  IRequestHandler<GetTasksQuery, GetTasksVm>
    {
        private readonly IAuthService _authService;
        private readonly IAuthInformationsHolder _holder;

        public GetTasksQueryHandler(IMyTasksDbContext context, IAuthService authService,
          IAuthInformationsHolder holder) : base(context)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _holder = holder;
        }

        public Task<GetTasksVm> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = _context.Tasks.Where(task => task.StatusId == Status.ActiveRecord);

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
                    OwnerName = _authService.GetUserByIdAsync(task.OwnerId).Result.Email,
                };
                tasksVm.TaskDtos.Add(adaptTask);
            });

            return Task.FromResult(tasksVm);
        }
    }
}
