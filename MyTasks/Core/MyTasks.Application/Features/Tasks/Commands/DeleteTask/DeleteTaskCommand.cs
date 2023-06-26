using MediatR;

namespace MyTasks.Application.Features.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommand : IRequest<int>
    {
        public int Id { get; set; }
    }
}
