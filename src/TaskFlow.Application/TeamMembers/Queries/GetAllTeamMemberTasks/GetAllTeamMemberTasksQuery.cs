using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Tasks.Commands.CreateTask;

namespace TaskFlow.Application.TeamMembers.Queries.GetAllTeamMemberTasks
{
    public class GetAllTeamMemberTasksQuery : IQuery<List<TaskResponse>>
    {
        public Guid TeamMemberId { get; init; }
    }
}
