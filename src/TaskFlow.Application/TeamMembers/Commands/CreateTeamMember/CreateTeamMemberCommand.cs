using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;


namespace TaskFlow.Application.TeamMembers.Commands.CreateTeamMember
{
    public class CreateTeamMemberCommand : ICommand
    {
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
    }
}
