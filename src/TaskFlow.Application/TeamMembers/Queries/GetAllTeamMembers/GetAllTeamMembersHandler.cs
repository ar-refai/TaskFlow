using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Common;
using TaskFlow.Application.TeamMembers.Commands.CreateTeamMember;
using TaskFlow.Domain;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Application.TeamMembers.Queries.GetAllTeamMembers
{
    public class GetAllTeamMembersHandler : IQueryHandler<GetAllTeamMembersQuery, List<TeamMemberResponse>>
    {
        private readonly ITeamMemberRepository _memberRepo;

        public GetAllTeamMembersHandler(ITeamMemberRepository memberRepo)
        {
            _memberRepo = memberRepo;
        }

        public async Task<Result<List<TeamMemberResponse>>> Handle(GetAllTeamMembersQuery query, CancellationToken cancellationToken = default)
        {
            var entities = await _memberRepo.GetAllAsync(cancellationToken);
            List<TeamMemberResponse> result = entities.Select(e => e.ToResponse()).ToList();
            return Result.Success(result);
        }
    }
}
