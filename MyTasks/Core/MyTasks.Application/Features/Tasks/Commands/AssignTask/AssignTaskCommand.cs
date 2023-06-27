using MediatR;

namespace MyTasks.Application.Features.Tasks.Commands.AssignTask
{
    public class AssignTaskCommand : IRequest<AssignTaskResult>
    {
        public string UserId { get; set; }
        public int TaskId { get; set; }
    }
}
