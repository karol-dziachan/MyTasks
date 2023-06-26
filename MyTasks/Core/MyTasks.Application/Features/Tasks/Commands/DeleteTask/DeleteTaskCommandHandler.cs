using MediatR;
using Microsoft.EntityFrameworkCore;
using MyTasks.Application.Abstraction;
using MyTasks.Application.Common.Interfaces;
using MyTasks.Common.Exceptions;
using MyTasks.Infrastructure.Services;

namespace MyTasks.Application.Features.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommandHandler : BaseRequestHandler, IRequestHandler<DeleteTaskCommand, int>
    {
        private readonly IAuthService _authService;
        private readonly IAuthInformationsHolder _holder;

        public DeleteTaskCommandHandler(IMyTasksDbContext context, IAuthService authService,
          IAuthInformationsHolder holder
            ) : base(context)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _holder = holder;
        }

        public async Task<int> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _context.Tasks
                .Where(task => task.Id == request.Id && task.StatusId == Status.ActiveRecord)
                .FirstOrDefaultAsync(cancellationToken);

            if (!_authService.GetUserInfo(_holder.IdToken).UserId.Contains(task.OwnerId))
            {
                throw new UserIsNotOwnerException();
            }

            if (task is null)
            {
                throw new ItemNotFoundException(request.Id.ToString(), nameof(Domain.Entities.Task));
            }

            _context.Tasks.Remove(task);

            await _context.SaveChangesAsync(cancellationToken);

            return task.Id;
        }
    }
}
