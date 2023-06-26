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

            // Obsłuż błąd odpowiedzi
            // Możesz go rzucić dalej lub zwrócić wartość domyślną
            // Przykładowa obsługa:
            // throw new Exception("Błąd w pobieraniu użytkowników z Auth0.");
            return null;
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

            // Obsłuż błąd odpowiedzi
            // Możesz go rzucić dalej lub zwrócić wartość domyślną, jeśli użytkownik nie został znaleziony
            // Przykładowa obsługa:
            // throw new Exception("Błąd w pobieraniu użytkownika z Auth0.");
            return null;
        }

        private string GetManagementApiToken(string domain, string clientId, string clientSecret)
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

            // Obsłuż błąd odpowiedzi
            // Możesz go rzucić dalej lub zwrócić wartość domyślną
            // Przykładowa obsługa:
            // throw new Exception("Błąd w uzyskiwaniu tokenu z Auth0.");
            return null;
        }

        private UserInfo MapToUserInfo(User user)
        {
            var userInfo = new UserInfo
            {
                UserId = user.UserId,
                NickName = user.NickName,
                Email = user.Email,
                // Dodaj inne informacje użytkownika
            };

            return userInfo;
        }

        private UserInfo DecodeUserInfo(string idToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(idToken);

            var userInfo = new UserInfo
            {
                UserId = token.Claims.FirstOrDefault(c => c.Type == "sub")?.Value,
                NickName = token.Claims.FirstOrDefault(c => c.Type == "name")?.Value,
                Email = token.Claims.FirstOrDefault(c => c.Type == "email")?.Value,
                // Dodaj inne informacje, które chcesz odczytać z tokena id_token
            };

            return userInfo;
        }

    }
}
