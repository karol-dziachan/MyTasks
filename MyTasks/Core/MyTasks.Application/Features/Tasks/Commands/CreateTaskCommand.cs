using MediatR;

namespace MyTasks.Application.Features.Tasks.Commands
{
    public class CreateTaskCommand : IRequest<CreateTaskResult>
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public DateTime? Duration { get; set; }
    }
}
