﻿using Auth0.AuthenticationApi.Models;

namespace MyTasks.Application.Common.Interfaces
{
    public interface IAuthService
    {
        UserInfo GetUserInfo(string idToken);
        Task<bool> CheckUserExistsAsync(string userId);
        Task<List<UserInfo>> GetAllUsersAsync();
        Task<UserInfo> GetUserByIdAsync(string userId);
    }
}
