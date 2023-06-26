using MediatR;


namespace MyTasks.Application.Features.Tasks.Queries.GetTasksByLoggedUser
{
    public class GetTasksByLoggedUserVm : IRequest<GetTaskDto>
    {
        public ICollection<GetTaskDto> TaskDtos { get; set; }

        public GetTasksByLoggedUserVm()
        {
            TaskDtos = new List<GetTaskDto>();
        }
    }
}
