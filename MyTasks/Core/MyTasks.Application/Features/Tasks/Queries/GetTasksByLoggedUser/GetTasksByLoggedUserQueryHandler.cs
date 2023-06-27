using MediatR;
using MyTasks.Application.Abstraction;
using MyTasks.Application.Common.Interfaces;
using MyTasks.Infrastructure.Services;

namespace MyTasks.Application.Features.Tasks.Queries.GetTasksByLoggedUser
{
    public class GetTasksByLoggedUserQueryHandler : BaseRequestHandler, IRequestHandler<GetTasksByLoggedUserQuery, GetTasksByLoggedUserVm>
    {
        private readonly IAuthService _authService;
        private readonly IAuthInformationsHolder _holder;

        public GetTasksByLoggedUserQueryHandler(IMyTasksDbContext context, IAuthService authService, IAuthInformationsHolder holder) : base(context)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _holder = holder ?? throw new ArgumentNullException(nameof(holder));
        }

        public Task<GetTasksByLoggedUserVm> Handle(GetTasksByLoggedUserQuery request, CancellationToken cancellationToken)
        {
            var userInfo = _authService.GetUserInfo(_holder.IdToken);
            var tasks = _context.Tasks.Where(task => task.OwnerId == userInfo.UserId);
            var assignTasksIds = _context.UsersTasks.Where(record => record.UserId == userInfo.UserId).Select(record => record.TaskId).ToList();
            var assignTasks = _context.Tasks.Where(task => assignTasksIds.Contains(task.Id) && task.OwnerId != userInfo.UserId);
            tasks = tasks.Concat(assignTasks);
            tasks = tasks.Where(task => task.StatusId == Status.ActiveRecord);

            if (!tasks.Any()) return default;

            var tasksVm = new GetTasksByLoggedUserVm();

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
