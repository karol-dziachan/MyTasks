using MyTasks.Application.Common.Interfaces;
using Auth0.AuthenticationApi.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using RestSharp;
using IdentityModel.Client;
using Auth0.ManagementApi.Models;

namespace MyTasks.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public UserInfo GetUserInfo(string idToken)
        {
            var userInfo = DecodeUserInfo(idToken);

            return userInfo;
        }

        public async Task<List<UserInfo>> GetAllUsers()
        {
            var domain = _configuration["Auth0:Domain"];
            var clientId = _configuration["Auth0:ClientId"];
            var clientSecret = _configuration["Auth0:ClientSecret"];

            var client = new RestClient($"https://{domain}/api/v2/");
            var request = new RestRequest("users", Method.Get);
            request.AddHeader("Authorization", $"Bearer {GetManagementApiToken(domain, clientId, clientSecret)}");

            var response = await client.ExecuteAsync<List<User>>(request);

            if (response.IsSuccessful)
            {
                var users = response.Data;
                var userInfos = users.Select(MapToUserInfo).ToList();
                return userInfos;
            }

           
            throw new Exception("Error fetching users from Auth0.");
        }

        public async Task<UserInfo> GetUserById(string userId)
        {
            var domain = _configuration["Auth0:Domain"];
            var clientId = _configuration["Auth0:ClientId"];
            var clientSecret = _configuration["Auth0:ClientSecret"];

            var client = new RestClient($"https://{domain}/api/v2/");
            var request = new RestRequest($"users/{userId}", Method.Get);
            request.AddHeader("Authorization", $"Bearer {GetManagementApiToken(domain, clientId, clientSecret)}");

            var response = await client.ExecuteAsync<User>(request);

            if (response.IsSuccessful)
            {
                var user = response.Data;
                var userInfo = MapToUserInfo(user);
                return userInfo;
            }


            throw new Exception("Error fetching user from Auth0.");
        }

        private static string GetManagementApiToken(string domain, string clientId, string clientSecret)
        {
            var client = new RestClient($"https://{domain}/oauth/token");
            var request = new RestRequest();
            request.Method = Method.Post;
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("client_id", clientId);
            request.AddParameter("client_secret", clientSecret);
            request.AddParameter("audience", $"https://{domain}/api/v2/");

            var response = client.Execute<TokenResponse>(request);

            if (response.IsSuccessful)
            {
                var tokenResponse = response.Data;
                return tokenResponse.AccessToken;
            }

            throw new Exception("Error fetching token from Auth0.");
        }

        private static UserInfo MapToUserInfo(User user)
        {
            var userInfo = new UserInfo
            {
                UserId = user.UserId,
                NickName = user.NickName,
                Email = user.Email,
                // Add next informations
            };

            return userInfo;
        }

        private static UserInfo DecodeUserInfo(string idToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(idToken);

            var userInfo = new UserInfo
            {
                UserId = token.Claims.FirstOrDefault(c => c.Type == "sub")?.Value,
                NickName = token.Claims.FirstOrDefault(c => c.Type == "name")?.Value,
                Email = token.Claims.FirstOrDefault(c => c.Type == "email")?.Value,
                // Add next informations
            };

            return userInfo;
        }

    }
}
