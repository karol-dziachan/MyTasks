using Microsoft.AspNetCore.Mvc;
using MyTasks.Api.Abstraction;
using MyTasks.Application.Features.Tasks.Commands;
using MyTasks.Application.Features.Tasks.Commands.CreateTask;
using MyTasks.Application.Features.Tasks.Commands.UpdateTask;
using MyTasks.Application.Features.Tasks.Queries.GetTasks;

namespace MyTasks.Api.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TasksController : BaseController
    {
        /// <summary>
        ///  Get all tasks
        /// </summary>
        /// <returns>Book</returns>
        /// <response code="200">If everything is ok</response>
        /// <response code="403">If the user is not authorization</response>
        /// <response code="404">If the book not found</response>
        [HttpGet("/all-tasks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetAllTasks()
        {
           return !User.Identity.IsAuthenticated ? Forbid("User is not authenticated") : Ok(await Mediator.Send(new GetTasksQuery()));
        }

        /// <summary>
        ///  Created new task
        /// </summary>
        /// <param name="taskCommand">New task</param>
        /// <returns>A newly created task id</returns>
        /// <response code="200">If the task was created</response>
        /// <response code="403">If the request is invalid</response>
        /// <response code="403">If the user is not authorization</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateTask(CreateTaskCommand taskCommand)
        {
            if (!User.Identity.IsAuthenticated) return Forbid("User is not authenticated");

            var vm = await Mediator.Send(taskCommand);

            if (vm == default)
            {
                return BadRequest(taskCommand);
            }

            return Ok(vm);
        }

        /// <summary>
        ///  Updated task
        /// </summary>
        /// <param name="taskCommand">Command for update task</param>
        /// <returns>A succesfully updated task id</returns>
        /// <response code="200">If the task was updated succesfully</response>
        /// <response code="403">If the request is invalid</response>
        /// <response code="403">If the user is not authorization</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateTask(UpdateTaskCommand taskCommand)
        {
            if (!User.Identity.IsAuthenticated) return Forbid("User is not authenticated");

            var vm = await Mediator.Send(taskCommand);

            if (vm == default)
            {
                return BadRequest(taskCommand);
            }

            return Ok(vm);
        }
    }
}
