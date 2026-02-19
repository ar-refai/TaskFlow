using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.TeamMembers.Commands.CreateTeamMember;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.TeamMembers
{
    public static class TeamMemberMappingExtensions
    {
        public static TeamMemberResponse ToResponse(this TeamMember member)
        {
            return new TeamMemberResponse
            {
                Id = member.Id.Value,
                Name = member.Name,
                Email = member.Email,
                CreatedAt = member.CreatedAt
            };
        }
    }
}
