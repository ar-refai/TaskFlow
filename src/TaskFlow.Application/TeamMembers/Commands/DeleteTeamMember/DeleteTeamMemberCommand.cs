using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;

namespace TaskFlow.Application.TeamMembers.Commands.DeleteTeamMember
{
    public class DeleteTeamMemberCommand : ICommand
    {
        public Guid TeamMemberId { get; init; }
    }
}
