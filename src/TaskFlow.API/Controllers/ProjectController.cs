using Microsoft.AspNetCore.Mvc;
using TaskFlow.API.Requests;
using TaskFlow.Application.Projects.Commands.CreateProject;
using TaskFlow.Application.Projects.Queries;

namespace TaskFlow.API.Controllers
{
    [ApiController]
    [Route("api/projects")]
    public class ProjectController : ControllerBase
    {
        private readonly CreateProjectHandler _createProjectHandler;
        private readonly GetProjectByIdHandler _getProjectByIdHandler;
        public ProjectController(CreateProjectHandler createProjectHandler, GetProjectByIdHandler getProjectByIdHandler)
        {
            _createProjectHandler = createProjectHandler;
            _getProjectByIdHandler = getProjectByIdHandler;
        }
        [HttpPost]
        [ProducesResponseType(typeof(ProjectResponse),StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
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

    }
}
