using Microsoft.AspNetCore.Mvc;
using TaskFlow.API.Requests;
using TaskFlow.Application.Projects.Commands.CreateProject;
using TaskFlow.Application.Projects.Commands.DeleteProject;
using TaskFlow.Application.Projects.Commands.UpdateProject;
using TaskFlow.Application.Projects.Queries;
using TaskFlow.Application.Projects.Queries.GetAllProjects;
using TaskFlow.Application.Projects.Queries.GetProjectById;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.API.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectController : ControllerBase
    {
        private readonly GetAllProjectsHandler _getAllProjectsHandler;
        private readonly GetProjectByIdHandler _getProjectByIdHandler;
        private readonly CreateProjectHandler _createProjectHandler;
        private readonly UpdateProjectHandler _updateProjectHandler;
        private readonly DeleteProjectHandler _deleteProjectHandler;

        public ProjectController(GetAllProjectsHandler getAllProjectsHandler, GetProjectByIdHandler getProjectByIdHandler , CreateProjectHandler createProjectHandler, UpdateProjectHandler updateProjectHandler, DeleteProjectHandler deleteProjectHandler)
        {
            _getAllProjectsHandler = getAllProjectsHandler;
            _getProjectByIdHandler = getProjectByIdHandler;
            _createProjectHandler = createProjectHandler;
            _updateProjectHandler = updateProjectHandler;
            _deleteProjectHandler = deleteProjectHandler;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ProjectResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllProjects(CancellationToken cancellationToken)
        {
            var query = new GetAllProjectsQuery();
            var result = await _getAllProjectsHandler.Handle(query, cancellationToken);
            if (result.IsFailure)
                return NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = result.Error
                });
            return Ok(result.Value);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ProjectResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjectById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetProjectByIdQuery { ProjectId = id };
            var result = await _getProjectByIdHandler.Handle(query, cancellationToken);
            if (result.IsFailure)
                return NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = result.Error
                });
            return Ok(result.Value);
        }


        [HttpPost]
        [ProducesResponseType(typeof(ProjectResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request,
        CancellationToken cancellationToken)
        {
            var command = new CreateProjectCommand
            {
                Name = request.Name,
                Description = request.Description
            };
            var result = await _createProjectHandler.Handle(command, cancellationToken);
            if (result.IsFailure)
                return BadRequest(new ProblemDetails
                {
                    Status = 400,
                    Title = result.Error
                });
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProject(Guid id ,[FromBody]UpdateProjectRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateProjectCommand
            {
                Id = id,
                Name = request.Name,
                Description = request.Description,
            };
            var result = await _updateProjectHandler.Handle(command,cancellationToken);
            if (result.IsFailure)
            {
                var statusCode = result.Error.Contains("not found") ? 404 : 400;
                return StatusCode(statusCode, new ProblemDetails
                {
                    Status = statusCode,
                    Title = result.Error
                });
            }
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProject(Guid id, CancellationToken cancellationToken)
        {
            DeleteProjectCommand command = new DeleteProjectCommand
            {
                ProjectId = id
            };
            var result = await _deleteProjectHandler.Handle(command, cancellationToken);
            if (result.IsFailure)
                return NotFound(new ProblemDetails{
                Status = 404,
                Title = result.Error
            });
            return NoContent();
        }



    }
}
