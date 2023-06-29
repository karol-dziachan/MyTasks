using MediatR;
using MyTasks.Application.Abstraction;
using MyTasks.Application.Common.Interfaces;

namespace MyTasks.Application.Features.Users.GetInformationsAboutLoggetUser
{
    public  class GetInformationsAboutLoggetUserCommandHandler : BaseRequestHandler, 
                                                           IRequestHandler<GetInformationsAboutLoggetUserCommand, GetInformationsAboutLoggetUserResult>
    {
        private readonly IAuthService _authService;
        private readonly IAuthInformationsHolder _holder;

        public GetInformationsAboutLoggetUserCommandHandler(IMyTasksDbContext context, IAuthService authService, IAuthInformationsHolder holder) : base(context)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _holder = holder ?? throw new ArgumentNullException(nameof(holder));
        }
        public Task<GetInformationsAboutLoggetUserResult> Handle(GetInformationsAboutLoggetUserCommand request, CancellationToken cancellationToken)
        {
            var token = _holder.GetToken();
            var user = string.IsNullOrEmpty(token) ? null :_authService.GetUserInfo(token);
            var result = new GetInformationsAboutLoggetUserResult()
            {
                User = user,
                IsAuthenticated = user is not null
            };

            return Task.FromResult(result);
        }
    }
}
