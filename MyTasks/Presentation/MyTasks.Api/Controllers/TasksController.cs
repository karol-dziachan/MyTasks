using Microsoft.AspNetCore.Mvc;
using MyTasks.Api.Abstraction;

namespace MyTasks.Api.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TasksController : BaseController
    {
        [HttpGet("/test")]
        public async Task<ActionResult> Test()
        {
            if (User.Identity.IsAuthenticated)
            {
                return Ok("Ok");
            }
            else
            {
                return BadRequest("user is not authenticated");
            }
        }
    }
}
