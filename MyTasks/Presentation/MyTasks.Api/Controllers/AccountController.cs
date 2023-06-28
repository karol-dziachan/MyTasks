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

        public AccountController(IAuthInformationsHolder holder)
        {
            _holder = holder;
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
            return View(new
            {
                Name = User.Identity.Name,
                EmailAddress = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                ProfileImage = User.Claims.FirstOrDefault(c => c.Type == "picture")?.Value
            });
        }

        [Authorize]
        [HttpGet]
        [Route("/account/logout")]
        public async Task Logout()
        {
            var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                .WithRedirectUri(Url.Action("Index", "Home", null, "https"))
                .Build();

            await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            Response.Cookies.Delete(".AspNetCore.Cookies");
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

            if (!authenticateResult.Properties.Items.TryGetValue("AuthState", out var state) || state != authState)
            {
                return BadRequest("Błąd uwierzytelniania.");
            }

            var token = authenticateResult.Properties.Items[".Token.id_token"];

            _holder.IdToken = token;

            return Redirect("http://localhost:3000/");
        }
    }
}
