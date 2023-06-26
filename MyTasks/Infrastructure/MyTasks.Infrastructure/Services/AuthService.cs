using MyTasks.Application.Common.Interfaces;
using Auth0.AuthenticationApi.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Auth0.ManagementApi.Models;
using Newtonsoft.Json;

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

        public async Task<List<UserInfo>> GetAllUsersAsync()
        {
            var accessToken = await GetManagementApiTokenAsync();
            var domain = _configuration["Auth0:Domain"];
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await httpClient.GetAsync($"https://{domain}/api/v2/users");

            if (response.IsSuccessStatusCode)
            {
                var usersJson = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<User>>(usersJson);
                var userDtos = users.Select(MapToUserInfo).ToList();
                return userDtos;
            }

            throw new Exception("Error fetching users from Auth0.");
        }

        public async Task<UserInfo> GetUserByIdAsync(string userId)
        {
            var accessToken = await GetManagementApiTokenAsync();
            var domain = _configuration["Auth0:Domain"];
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await httpClient.GetAsync($"https://{domain}/api/v2/users/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var userJson = await response.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<User>(userJson);
                var userDto = MapToUserInfo(user);
                return userDto;
            }

            throw new Exception("Error fetching user nick from Auth0.");
        }


        public async Task<string> GetManagementApiTokenAsync()
        {
            var clientId = _configuration["Auth0:ClientId"];
            var clientSecret = _configuration["Auth0:ClientSecret"];
            var domain = _configuration["Auth0:Domain"];
            
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://{domain}/oauth/token");
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "audience", $"https://{domain}/api/v2/" }
            });

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var tokenResponseJson = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<dynamic>(tokenResponseJson);
                return tokenResponse.access_token;
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
