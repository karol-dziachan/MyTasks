using MediatR;

namespace MyTasks.Application.Features.Tasks.Queries.GetTasks
{
    public class GetTasksVm : IRequest<GetTaskDto>
    {
        public ICollection<GetTaskDto> TaskDtos { get; set; }

        public GetTasksVm()
        {
            TaskDtos = new List<GetTaskDto>();
        }
    }
}
