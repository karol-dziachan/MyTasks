using Auth0.AuthenticationApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MyTasks.Api.Abstraction;
using MyTasks.Application.Common.Interfaces;
using MyTasks.Application.Features.Users.GetInformationsAboutLoggetUser;

namespace MyTasks.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IAuthService _authService;
        
        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        ///  Get all users
        /// </summary>
        /// <returns>Users</returns>
        /// <response code="200">If everything is ok</response>
        /// <response code="403">If the user is not authorization</response>
        /// <response code="404">If the book not found</response>
        [HttpGet("/all-users")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<UserInfo>>> GetAllUsers()
        {
            if (!User.Identity.IsAuthenticated) return Forbid("User is not authenticated");

            var users = await _authService.GetAllUsersAsync();

            if (!users.Any()) return NotFound();

            return Ok(users);
        }

        /// <summary>
        ///  Get information about user
        /// </summary>
        /// <returns>Bool</returns>
        /// <response code="200">Always</response>
        [HttpGet("/user-authenticated")]
        [EnableCors("AllowOrigin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> IsUferAuthenticated( )
        {
            var vm = await Mediator.Send(new GetInformationsAboutLoggetUserCommand(){  });

            return Ok(vm.IsAuthenticated);  
        }

        /// <summary>
        ///  Get information about user
        /// </summary>
        /// <returns>Bool</returns>
        /// <response code="200">Always</response>
        /// <response code="403">If the user is not authorization</response>
        [HttpGet("/user-informations")]
        [EnableCors("AllowOrigin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<bool>> UserInformations()
        {
            var vm = await Mediator.Send(new GetInformationsAboutLoggetUserCommand() { });

            return vm.IsAuthenticated ? Ok(vm.User) : Forbid();
        }
    }
}
