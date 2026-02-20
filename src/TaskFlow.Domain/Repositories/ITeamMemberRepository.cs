using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Entities;
using TaskEntity = TaskFlow.Domain.Entities.Task;

using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Domain.Repositories
{
    public interface ITeamMemberRepository
    {
        Task<TeamMember?> GetByIdAsync(TeamMemberId teamMemberId, CancellationToken cancellationToken = default);
        Task<TeamMember?> GetByEmailAsync(Email email , CancellationToken cancellationToken = default);
        Task<List<TeamMember>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<List<TaskEntity>> GetAllTeamMemberTasks(TeamMemberId teamMemberId, CancellationToken cancellationToken = default);

        void Add(TeamMember teamMember);
        void Remove(TeamMember teamMember);
    }
}
