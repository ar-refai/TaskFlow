using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.TeamMembers.Commands.CreateTeamMember
{
    public class TeamMemberResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;

        public DateTime CreatedAt { get; init; }
    }
}
