﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using MyTasks.Application.Abstraction;
using MyTasks.Application.Common.Interfaces;
using MyTasks.Common.Exceptions;
using MyTasks.Infrastructure.Services;

namespace MyTasks.Application.Features.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommandHandler : BaseRequestHandler, IRequestHandler<UpdateTaskCommand, UpdateTaskResult>
    {
        private readonly IAuthService _authService;
        private readonly IAuthInformationsHolder _holder;

        public UpdateTaskCommandHandler(IMyTasksDbContext context, IAuthService authService,
          IAuthInformationsHolder holder
            ) : base(context)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _holder = holder;
        }
        public async Task<UpdateTaskResult> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _context.Tasks
                .Where(task => task.Id == request.Id && task.StatusId == Status.ActiveRecord)
                .FirstOrDefaultAsync(cancellationToken);

            if(task is null)
            {
                throw new ItemNotFoundException(request.Id, nameof(Domain.Entities.Task));
            }

            var userInfo = _authService.GetUserInfo(_holder.IdToken);
            var taskDto = new Domain.Entities.Task()
            {
                Id = task.Id,
                Title = task.Title,
                Content = task.Content,
                StartDateTime = task.StartDateTime,
                EndDateTime = task.EndDateTime,
                Duration = task.Duration,
                OwnerId = userInfo.UserId ?? throw new ArgumentNullException(nameof(userInfo.UserId)),
            };

            _context.Tasks.Update(taskDto);

            await _context.SaveChangesAsync(cancellationToken);

            return new UpdateTaskResult() { Id = task.Id };
        }
    }
}