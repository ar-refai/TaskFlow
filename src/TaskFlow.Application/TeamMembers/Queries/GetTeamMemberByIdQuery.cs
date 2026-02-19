using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.TeamMembers.Commands.CreateTeamMember;

namespace TaskFlow.Application.TeamMembers.Queries
{
    public class GetTeamMemberByIdQuery : IQuery<TeamMemberResponse>
    {
        public Guid TeamMemberId { get; init; }
    }
}
