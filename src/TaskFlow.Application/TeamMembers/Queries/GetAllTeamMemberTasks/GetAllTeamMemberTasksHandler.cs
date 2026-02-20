using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Common;
using TaskFlow.Application.Tasks;
using TaskFlow.Application.Tasks.Commands.CreateTask;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Application.TeamMembers.Queries.GetAllTeamMemberTasks
{
    public class GetAllTeamMemberTasksHandler : IQueryHandler<GetAllTeamMemberTasksQuery, List<TaskResponse>>
    {
        private readonly ITeamMemberRepository _memberRepo;
        
        public GetAllTeamMemberTasksHandler(ITeamMemberRepository memberRepo) 
        {
            _memberRepo = memberRepo;      
        }
        public async Task<Result<List<TaskResponse>>> Handle(GetAllTeamMemberTasksQuery query , CancellationToken cancellationToken = default)
        {
            var memberId = new TeamMemberId(query.TeamMemberId);
            var member = await _memberRepo.GetByIdAsync(memberId, cancellationToken);
            if (member is null)
                return Result.Failure<List<TaskResponse>>("Member not found");
            var entities = await _memberRepo.GetAllTeamMemberTasks(memberId, cancellationToken);
            var result = entities.Select(e => e.ToResponse()).ToList();
            return Result.Success(result);
        }
    }
}
