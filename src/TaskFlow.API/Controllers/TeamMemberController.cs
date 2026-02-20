using Microsoft.AspNetCore.Mvc;
using TaskFlow.API.Requests.TeamMembers;
using TaskFlow.Application.Tasks.Commands.CreateTask;
using TaskFlow.Application.TeamMembers.Commands.CreateTeamMember;
using TaskFlow.Application.TeamMembers.Commands.DeleteTeamMember;
using TaskFlow.Application.TeamMembers.Commands.UpdateTeamMember;
using TaskFlow.Application.TeamMembers.Queries;
using TaskFlow.Application.TeamMembers.Queries.GetAllTeamMembers;
using TaskFlow.Application.TeamMembers.Queries.GetAllTeamMemberTasks;
using TaskFlow.Application.TeamMembers.Queries.GetTeamMemberById;

namespace TaskFlow.API.Controllers
{
    [ApiController]
    [Route("api/team-members")]
    public class TeamMemberController : ControllerBase
    {
        private readonly GetAllTeamMembersHandler _getAllTeamMembersHandler;
        private readonly GetTeamMemberByIdHandler _getTeamMemberByIdHandler;
        private readonly GetAllTeamMemberTasksHandler _getAllTeamMemberTasksHandler;
        private readonly CreateTeamMemberHandler _createTeamMemberHandler;
        private readonly UpdateTeamMemberHandler _updateTeamMemberHandler;
        private readonly DeleteTeamMemberHandler _deleteTeamMemberHandler;

        public TeamMemberController(GetAllTeamMembersHandler getAllTeamMembersHandler, GetTeamMemberByIdHandler getTeamMemberByIdHandler, CreateTeamMemberHandler createTeamMemberHandler, UpdateTeamMemberHandler updateTeamMemberHandler, DeleteTeamMemberHandler deleteTeamMemberHandler, GetAllTeamMemberTasksHandler getAllTeamMemberTasksHandler)
        {
            _getAllTeamMembersHandler = getAllTeamMembersHandler;
            _getTeamMemberByIdHandler = getTeamMemberByIdHandler;
            _getAllTeamMemberTasksHandler = getAllTeamMemberTasksHandler;
            _createTeamMemberHandler = createTeamMemberHandler; 
            _updateTeamMemberHandler = updateTeamMemberHandler;
            _deleteTeamMemberHandler = deleteTeamMemberHandler;
        }


        [HttpGet]
        [ProducesResponseType(typeof(List<TeamMemberResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllTeamMembers(CancellationToken cancellationToken)
        {
            var query = new GetAllTeamMembersQuery();
            var result = await _getAllTeamMembersHandler.Handle(query, cancellationToken);
            return Ok(result);
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


        [HttpGet("{id:guid}/tasks")]
        [ProducesResponseType(typeof(List<TaskResponse>),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllTeamMemberTasks(Guid memberId,  CancellationToken cancellationToken)
        {
            var query = new GetAllTeamMemberTasksQuery
            {
                TeamMemberId = memberId,
            };
            var result = await _getAllTeamMemberTasksHandler.Handle(query, cancellationToken);
            if (result.IsFailure) 
                return NotFound("Team member not found.");
            return Ok(result.Value);
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


        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTeamMember(Guid id, UpdateTeamMemberRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateTeamMemberCommand
            {
                Name  = request.Name ,
                Email = request.Email
            };
            var result = await _updateTeamMemberHandler.Handle(command, cancellationToken);
            if(result.IsFailure)
            {
                var code = result.Error.Contains("not found") ? 404 : 400;
                return StatusCode(code, new ProblemDetails
                {
                    Status = code,
                    Title = result.Error
                });
            }
            return NoContent();
        }


        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails),StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteTeamMember(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteTeamMemberCommand
            {
                TeamMemberId = id ,
            };
            var result = await _deleteTeamMemberHandler.Handle(command, cancellationToken);
            if (result.IsFailure)
                return NotFound(new ProblemDetails
                {
                    Status = 404,
                    Title = result.Error
                });
            return NoContent();   
        }

    }
}
