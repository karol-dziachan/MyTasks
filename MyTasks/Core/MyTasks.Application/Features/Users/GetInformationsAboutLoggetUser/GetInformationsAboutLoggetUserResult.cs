using Auth0.AuthenticationApi.Models;

namespace MyTasks.Application.Features.Users.GetInformationsAboutLoggetUser
{
    public class GetInformationsAboutLoggetUserResult
    {
        public UserInfo User { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
