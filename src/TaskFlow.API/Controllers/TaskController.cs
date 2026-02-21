using Microsoft.AspNetCore.Mvc;
using TaskFlow.API.Requests.Tasks;
using TaskFlow.Application.Common;
using TaskFlow.Application.Tasks.Commands.AddCommentToTask;
using TaskFlow.Application.Tasks.Commands.AddRemoveTagsInTask;
using TaskFlow.Application.Tasks.Commands.AssignTask;
using TaskFlow.Application.Tasks.Commands.ChangeTaskStatus;
using TaskFlow.Application.Tasks.Commands.CreateTask;
using TaskFlow.Application.Tasks.Commands.DeleteTask;
using TaskFlow.Application.Tasks.Commands.UpdateTask;
using TaskFlow.Application.Tasks.Queries;
using TaskFlow.Application.Tasks.Queries.GetTaskById;
using TaskFlow.Application.Tasks.Queries.GetTasksByProject;
using TaskFlow.Application.Tasks.Queries.GetTasksWithFilters;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.API.Controllers
{
    [ApiController]
    [Route("api/")]
    public class TaskController : ControllerBase
    {
        private readonly GetTaskByIdHandler _getTask;
        private readonly GetTasksWithFiltersHandler _getTasksWithFilter;
        private readonly GetTasksByProjectHandler _getTasksByProject;
        private readonly ChangeTaskStatusHandler _changeStatus;
        private readonly AssignTaskHandler _assignTask;
        private readonly CreateTaskHandler _createTask;
        private readonly UpdateTaskHandler _updateTask;
        private readonly DeleteTaskHandler _deleteTask;
        private readonly AddRemoveTagsInTaskHandler _addRemoveTagsInTask;
        private readonly AddCommentToTaskHandler _addCommentToTask;

        public TaskController
        (
            GetTasksWithFiltersHandler getTasksWithFilter,
            GetTasksByProjectHandler getTasksByProject,
            GetTaskByIdHandler getTask,
            ChangeTaskStatusHandler changeStatus,
            AssignTaskHandler assignTask,
            CreateTaskHandler createTask,
            UpdateTaskHandler updateTask,
            DeleteTaskHandler deleteTask,
            AddRemoveTagsInTaskHandler addRemoveTagsInTask,
            AddCommentToTaskHandler addCommentToTask
        )
        {
            _getTasksWithFilter = getTasksWithFilter;
            _getTasksByProject = getTasksByProject;
            _getTask = getTask;
            _changeStatus = changeStatus;
            _assignTask = assignTask;
            _addCommentToTask = addCommentToTask;
            _addRemoveTagsInTask = addRemoveTagsInTask;
            _createTask = createTask;
            _updateTask = updateTask;
            _deleteTask = deleteTask;
        }


        // GET /api/tasks?status=InProgress&assigneeId=...&priority=High
        [HttpGet("tasks")]
        [ProducesResponseType(typeof(List<TaskResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTasksWithFilters
            (
                [FromQuery] string? status,
                [FromQuery] string? priority,
                [FromQuery] Guid assigneeId,
                CancellationToken cancellationToken
            )
        {
            var query = new GetTasksWithFiltersQuery
            {
                Status = status,
                Priority = priority,
                AssigneeId = assigneeId,
            };
            var result = await _getTasksWithFilter.Handle(query, cancellationToken);
            return Ok(result.Value);
        }

        [HttpGet("projects/{projectId:guid}/tasks")]
        [ProducesResponseType(typeof(List<TaskResponse>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTasksByProject
            (
                Guid projectId,
                CancellationToken cancellationToken
            )
        {
            var query = new GetTasksByProjectQuery
            {
                ProjectId = projectId,
            };
            var result = await _getTasksByProject.Handle(query, cancellationToken);
            return Ok(result.Value);

        }

        [HttpGet("tasks/{id:guid}")]
        [ProducesResponseType(typeof(TaskResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTaskById
            (
            Guid id,
            CancellationToken cancellationToken
            )
        {
            var query = new GetTaskByIdQuery
            {
                TaskId = id
            };
            var result = await _getTask.Handle(query, cancellationToken);
            if (result.IsFailure) return NotFound(new ProblemDetails
            {
                Status = 404,
                Title = result.Error
            });
            return Ok(result.Value);
        }

        [HttpPut("tasks/{id:guid}/status")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangeTaskStatus
            (
            Guid id,
            [FromBody] ChangeTaskStatusRequest request,
            CancellationToken cancellationToken
            )
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

        [HttpPut("tasks/{id:guid}/assign")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AssignTask
            (
            Guid id,
            [FromBody] AssignTaskRequest request,
            CancellationToken cancellationToken
            )
        {
            var command = new AssignTaskCommand
            {
                TaskId = id,
                TeamMemberId = request.TeamMemberId
            };
            var result = await _assignTask.Handle(command, cancellationToken);
            if (result.IsFailure) return BadRequest(new ProblemDetails
            {
                Status = 400,
                Title = result.Error
            });

            return NoContent();
        }

        [HttpPost("tasks/{id:guid}/comments")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddComment
            (
            Guid id,
            AddCommentRequest request,
            CancellationToken cancellationToken
            )
        {
            var command = new AddCommentToTaskCommand
            {
                TaskId = id,
                TeamMemberId = request.AuthorId,
                Content = request.Content
            };
            var result = await _addCommentToTask.Handle(command, cancellationToken);
            if (result.IsFailure)
                return BadRequest(new ProblemDetails 
                { 
                Status = 400,
                Title = result.Error
                });
            return NoContent();

        }

        [HttpPut("tasks/{id:guid}/tags")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTaskTags(
            Guid id,
            [FromBody] AddRemoveTagsRequest request,
            CancellationToken cancellationToken)
        {
            var command = new AddRemoveTagsInTaskCommand
            {
                TaskId = id,
                Tags = request.Tags
            };

            var result = await _addRemoveTagsInTask.Handle(command, cancellationToken);

            if (result.IsFailure)
                return BadRequest(new ProblemDetails { Status = 400, Title = result.Error });

            return NoContent();
        }


        [HttpPost("projects/{projectId}/tasks")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTask
            (
            Guid projectId,
            [FromBody] CreateTaskRequest request,
            CancellationToken cancellationToken
            )
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
            if (result.IsFailure)
                return BadRequest(new ProblemDetails
                {
                    Status = 400,
                    Title = result.Error
                });
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("tasks/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTask
            (
            Guid id,
            [FromBody] UpdateTaskRequest request,
            CancellationToken cancellationToken
            )
        {
            var command = new UpdateTaskCommand
            {
                TaskId = id,
                Title = request.Title,  
                Description = request.Description,
                Priority = request.Priority,
                DateRange = request.DateRange is not null ? new DateRangeDto {
                    StartDate = request.DateRange.StartDate, 
                    DueDate = request.DateRange.DueDate
                } : null
            };
            var result = await _updateTask.Handle(command, cancellationToken);
            if(result.IsFailure)
            {
                var errorStatus = result.Error.Contains("not found") ? 404 : 400;
                return StatusCode(errorStatus,new ProblemDetails
                {
                    Status = errorStatus,
                    Title = result.Error
                });

            }
            return NoContent();
        }

        [HttpDelete("tasks/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTask
            (
            Guid id,
            CancellationToken cancellationToken
            )
        {
            DeleteTaskCommand command = new DeleteTaskCommand
            {
                TaskId = id
            };
            var result = await _deleteTask.Handle(command, cancellationToken);
            if (result.IsFailure) 
            { 
                var code = result.Error.Contains("not found") ? 404 : 400;
                return StatusCode(code, new ProblemDetails {
                    Status = code,
                    Title = result.Error
                });
            }
            return NoContent();
        }

    }
}
