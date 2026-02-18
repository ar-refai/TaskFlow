using Microsoft.AspNetCore.Mvc;
using TaskFlow.API.Requests;
using TaskFlow.Application.Common;
using TaskFlow.Application.Tasks.Commands.AssignTask;
using TaskFlow.Application.Tasks.Commands.ChangeTaskStatus;
using TaskFlow.Application.Tasks.Commands.CreateTask;
using TaskFlow.Application.Tasks.Queries;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.API.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly CreateTaskHandler _createTask;
        private readonly ChangeTaskStatusHandler _changeStatus;
        private readonly AssignTaskHandler _assignTask;
        private readonly GetTaskByIdHandler _getTask;

        public TaskController(CreateTaskHandler createTask, ChangeTaskStatusHandler changeStatus, AssignTaskHandler assignTask, GetTaskByIdHandler getTask)
        {
            _createTask = createTask;
            _changeStatus = changeStatus;
            _assignTask = assignTask;
            _getTask = getTask;
        }
        
        [HttpPost("projects/{projectId}/tasks")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTask(Guid projectId,[FromBody] CreateTaskRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateTaskCommand
            {
                ProjectId = projectId,
                Title = request.Title,
                Description = request.Description,
                Priority = request.Priority,
                DateRange = request.DateRange is not null ? new DateRangeDto 
                { 
                    StartDate = request.DateRange.StartDate,
                    DueDate = request.DateRange.DueDate 
                } : null,
                Tags = request.Tags,
            };
            var result = await _createTask.Handle(command, cancellationToken);
            if(result.IsFailure) 
                return BadRequest (new ProblemDetails
                {
                    Status = 400,
                    Title = result.Error
                });
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet("tasks/{id:guid}")]
        [ProducesResponseType(typeof(TaskResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTaskById(Guid id, CancellationToken cancellationToken)
        {
            var taskId = new TaskId(id);
            var query = new GetTaskByIdQuery
            {
                TaskId = id
            };
            var task = await _getTask.Handle(query, cancellationToken);
            if (task is null) return NotFound(new ProblemDetails
            {
                Status = 404,
                Title = task.Error
            });
            return Ok(task.Value);
        }

        [HttpPut("tasks/{id:guid}/assign")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AssignTask(Guid id,[FromBody] AssignTaskRequest request, CancellationToken cancellationToken)
        {
            var command = new AssignTaskCommand
            {
                TaskId = id,
                TeamMemberId = request.TeamMemberId
            };
           var result = await _assignTask.Handle(command, cancellationToken);
            if (result.IsFailure) return NotFound(new ProblemDetails
            {
                Status = 404,
                Title = result.Error
            });

            return NoContent();
        }

        [HttpPut("tasks/{id:guid}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeTaskStatus(Guid id,[FromBody] ChangeTaskStatusRequest request, CancellationToken cancellationToken)
        {
            var command = new ChangeTaskStatusCommand
            {
                TaskId = id,
                newStatus = request.NewStatus
            };
            var result = await _changeStatus.Handle(command, cancellationToken);
            if (result.IsFailure) return BadRequest(new ProblemDetails
            {
                Status = 400,
                Title = result.Error
            });

            return NoContent();
        }
    }
}
