using MediatR;

namespace MyTasks.Application.Features.Tasks.Queries.GetTasksByLoggedUser
{
    public class GetTasksByLoggedUserQuery : IRequest<GetTasksByLoggedUserVm>
    {
    }
}
