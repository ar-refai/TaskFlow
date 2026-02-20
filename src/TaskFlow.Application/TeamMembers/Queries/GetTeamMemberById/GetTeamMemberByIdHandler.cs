using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Common;
using TaskFlow.Application.TeamMembers.Commands.CreateTeamMember;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Application.TeamMembers.Queries.GetTeamMemberById
{
    public class GetTeamMemberByIdHandler : IQueryHandler<GetTeamMemberByIdQuery, TeamMemberResponse>
    {
        private ITeamMemberRepository _teamMemberRepo;
        public GetTeamMemberByIdHandler(ITeamMemberRepository teamMemberRepository)
        {
            _teamMemberRepo = teamMemberRepository;
        }

        public async Task<Result<TeamMemberResponse>> Handle(GetTeamMemberByIdQuery query, CancellationToken cancellationToken = default)
        {
            var tmId = new TeamMemberId(query.TeamMemberId);
            
            var teamMember = await _teamMemberRepo.GetByIdAsync(tmId);
            
            if (teamMember is null)
                return Result.Failure<TeamMemberResponse>("Team member not found.");

            var result = teamMember.ToResponse();

            return Result.Success(result);
        }
    }
}
