using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MyTasks.Application.Common.Interfaces;

namespace MyTasks.Api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAuthInformationsHolder _holder;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(IAuthInformationsHolder holder, IHttpContextAccessor httpContextAccessor)
        {
            _holder = holder;
            _httpContextAccessor = httpContextAccessor;
        }

        [Route("/account/signup")]
        [HttpGet]
        public async Task Signup(string returnUrl = "/")
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                .WithParameter("screen_hint", "signup")
                .WithRedirectUri(returnUrl)
                .Build();

            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        }

        [Route("/account/login")]
        [HttpGet]
        public IActionResult Login(string returnUrl = "/account/callback")
        {
            var state = Guid.NewGuid().ToString();

            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = returnUrl,
                Items =
        {
            { "AuthState", state }
        }
            };

            Response.Cookies.Append("AuthState", state);

            return Challenge(authenticationProperties, Auth0Constants.AuthenticationScheme);
        }

        [Authorize]
        [HttpGet]
        [Route("/account/profile")]
        public IActionResult Profile()
        {
            var name = User.Identity.Name;
            var emailAddress = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var profileImage = User.Claims.FirstOrDefault(c => c.Type == "picture")?.Value;

            HttpContext.Session.SetString("UserName", name);
            HttpContext.Session.SetString("UserEmailAddress", emailAddress);
            HttpContext.Session.SetString("UserProfileImage", profileImage);

            return View(new
            {
                Name = name,
                EmailAddress = emailAddress,
                ProfileImage = profileImage
            });
        }

        [Authorize]
        [HttpGet]
        [Route("/account/logout")]
        public async Task<IActionResult> Logout()
        {
            var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                .WithRedirectUri("http://localhost:3000/")
                .Build();

            Response.Cookies.Delete("IdToken");

            if (_httpContextAccessor.HttpContext.Request.Cookies.ContainsKey("IdToken"))
            {
                Response.Cookies.Append("IdToken", "", new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(-1),
                    Path = "/",
                    SameSite = SameSiteMode.None,
                    Secure = true
                });

            }

            await HttpContext.Session.LoadAsync();
            HttpContext.Session.Clear();

            return Challenge(authenticationProperties, Auth0Constants.AuthenticationScheme);
        }

        [HttpGet]
        [Route("/account/callback")]
        public async Task<IActionResult> Callback()
        {
            var authState = Request.Cookies["AuthState"];

            var authenticateResult = await HttpContext.AuthenticateAsync(Auth0Constants.AuthenticationScheme);
            if (!authenticateResult.Succeeded)
            {
                return BadRequest("Błąd uwierzytelniania.");
            }

            var token = authenticateResult.Properties.Items[".Token.id_token"];

            Response.Cookies.Append("IdToken", token, new CookieOptions
            {
                SameSite = SameSiteMode.None,
                Secure = true 
            });

            await HttpContext.Session.LoadAsync();
            _holder.IdToken = token;

            return Redirect("http://localhost:3000/");
            //return Redirect("/swagger");
        }
    }
}
