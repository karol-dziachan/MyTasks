using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MyTasks.Application.Abstraction;
using MyTasks.Application.Common.Interfaces;

namespace MyTasks.Application.Features.Tasks.Commands
{
    public class CreateTaskCommandHandler : BaseRequestHandler, IRequestHandler<CreateTaskCommand, CreateTaskResult>
    {
        private readonly IAuthService _authService;
        private readonly IAuthInformationsHolder _holder;
        // private readonly IServiceProvider _serviceProvider;

        public CreateTaskCommandHandler(IMyTasksDbContext context, IAuthService authService, 
          //  IServiceProvider serviceProvider
          IAuthInformationsHolder holder
            ) : base(context)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            //_serviceProvider = serviceProvider;
            _holder = holder;
        }

        public async Task<CreateTaskResult> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var userInfo = _authService.GetUserInfo(_holder.IdToken);

            var task = new Domain.Entities.Task()
            {
                Title = request.Title,
                Content = request.Content,
                StartDateTime = request.StartDateTime,
                EndDateTime = request.EndDateTime,
                Duration = request.Duration,
                OwnerName = userInfo.UserId ?? throw new ArgumentNullException(nameof(userInfo.UserId)),
            };

            _context.Tasks.Add(task);

            await _context.SaveChangesAsync(cancellationToken);

            return new CreateTaskResult() { Id = task.Id };
        }
    }
}
