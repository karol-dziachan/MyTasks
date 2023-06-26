using Auth0.AuthenticationApi.Models;

namespace MyTasks.Application.Common.Interfaces
{
    public interface IAuthService
    {
        UserInfo GetUserInfo(string idToken);
        Task<List<UserInfo>> GetAllUsers();
        Task<UserInfo> GetUserById(string userId);
    }
}
