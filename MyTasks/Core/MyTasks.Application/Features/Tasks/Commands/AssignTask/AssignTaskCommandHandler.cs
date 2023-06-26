using MediatR;
using Microsoft.EntityFrameworkCore;
using MyTasks.Application.Abstraction;
using MyTasks.Application.Common.Interfaces;
using MyTasks.Common.Exceptions;
using MyTasks.Infrastructure.Services;

namespace MyTasks.Application.Features.Tasks.Commands.AssignTask
{
    public class AssignTaskCommandHandler : BaseRequestHandler, IRequestHandler<AssignTaskCommand, AssignTaskResult>
    {
        private readonly IAuthService _authService;
        private readonly IAuthInformationsHolder _holder;

        public AssignTaskCommandHandler(IMyTasksDbContext context, IAuthService authService,
          IAuthInformationsHolder holder
            ) : base(context)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _holder = holder;
        }

        public async Task<AssignTaskResult> Handle(AssignTaskCommand request, CancellationToken cancellationToken)
        {
            var isUserExists = await _authService.CheckUserExistsAsync(request.UserId);
            var task = await _context.Tasks
                .Where(task => task.Id == request.TaskId && task.StatusId == Status.ActiveRecord)
                .FirstOrDefaultAsync(cancellationToken);

            if (!isUserExists) throw new ItemNotFoundException(request.UserId, "User");
            if (task is null) throw new ItemNotFoundException(request.TaskId.ToString(), nameof(request.TaskId));

            var usersTasks = new Domain.Entities.UsersTasks()
            {
                UserId = request.UserId,
                TaskId = request.TaskId
            };

            _context.UsersTasks.Add(usersTasks);

            await _context.SaveChangesAsync(cancellationToken);

            return new AssignTaskResult() { Id = usersTasks.Id };
        }
    }
}
