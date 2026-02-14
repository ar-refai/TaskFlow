using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Domain.Repositories
{
    public interface ITeamMemberRepository
    {
        Task<TeamMember?> GetByIdAsync(TeamMember teamMember, CancellationToken cancellationToken = default);
        Task<TeamMember?> GetByEmail(Email email , CancellationToken cancellationToken = default);
        Task<List<TeamMember>> GetAll(CancellationToken cancellationToken);

        void Add(TeamMember teamMember);
        void Remove(TeamMember teamMember);
    }
}
