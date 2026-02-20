using Microsoft.AspNetCore.Mvc;
using TaskFlow.API.Requests.TeamMembers;
using TaskFlow.Application.TeamMembers.Commands.CreateTeamMember;
using TaskFlow.Application.TeamMembers.Queries;
using TaskFlow.Application.TeamMembers.Queries.GetTeamMemberById;

namespace TaskFlow.API.Controllers
{
    [ApiController]
    [Route("api/team-members")]
    public class TeamMemberController : ControllerBase
    {
        private readonly CreateTeamMemberHandler _createTeamMemberHandler;
        private readonly GetTeamMemberByIdHandler _getTeamMemberByIdHandler;
        public TeamMemberController( CreateTeamMemberHandler createTeamMemberHandler, GetTeamMemberByIdHandler getTeamMemberByIdHandler )
        {
            _createTeamMemberHandler = createTeamMemberHandler; 
            _getTeamMemberByIdHandler = getTeamMemberByIdHandler;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTeamMember(
            [FromBody] CreateTeamMemberRequest request,
            CancellationToken cancellationToken)
        {
            var command = new CreateTeamMemberCommand { Name = request.Name ,Email = request.Email  };
            var result = await _createTeamMemberHandler.Handle(command,cancellationToken);
            if (result.IsFailure) 
                return BadRequest(new ProblemDetails
                {
                    Status = 400,
                    Title = result.Error
                });
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(TeamMemberResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTeamMemberById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var query = new GetTeamMemberByIdQuery
            {
                TeamMemberId = id
            };
            var result = await _getTeamMemberByIdHandler.Handle(query, cancellationToken);
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
