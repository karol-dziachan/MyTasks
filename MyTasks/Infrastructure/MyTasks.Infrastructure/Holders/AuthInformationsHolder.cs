using Microsoft.AspNetCore.Http;
using MyTasks.Application.Common.Interfaces;

namespace MyTasks.Infrastructure.Holders
{
    public class AuthInformationsHolder : IAuthInformationsHolder
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthInformationsHolder(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string IdToken
        {
            get => _httpContextAccessor.HttpContext.Session.GetString("IdToken") ?? string.Empty;
            set => _httpContextAccessor.HttpContext.Session.SetString("IdToken", value);
        }

        public string GetToken()
        {
            string sessionCookie = _httpContextAccessor.HttpContext.Request.Cookies["IdToken"];

            return sessionCookie;
        }
    }
}
