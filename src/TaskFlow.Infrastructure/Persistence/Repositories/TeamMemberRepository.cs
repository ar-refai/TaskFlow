using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.ValueObjects;
using TaskEntity = TaskFlow.Domain.Entities.Task;

namespace TaskFlow.Infrastructure.Persistence.Repositories
{
    public class TeamMemberRepository : ITeamMemberRepository
    {
        private readonly TaskFlowDbContext _context;
        public TeamMemberRepository(TaskFlowDbContext context)
        {
            _context = context;
        }

        public async Task<List<TeamMember>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.TeamMembers.AsNoTracking().OrderByDescending(t => t.Name).ToListAsync(cancellationToken);   
        }

        public async Task<List<TaskEntity>> GetAllTeamMemberTasks (TeamMemberId teamMemberId, CancellationToken cancellationToken =default)
        {
            return await _context.Tasks.AsNoTracking().OrderByDescending(t => t.CreatedAt).Where(t => t.AssignedTo == teamMemberId).ToListAsync(cancellationToken);
        }

        public async Task<TeamMember?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
        {
            return await _context.TeamMembers.AsNoTracking().FirstOrDefaultAsync(tm => tm.Email == email,cancellationToken);
        }

        public async Task<TeamMember?> GetByIdAsync(TeamMemberId teamMemberId, CancellationToken cancellationToken = default)
        {
            return await _context.TeamMembers.FirstOrDefaultAsync(tm => tm.Id == teamMemberId, cancellationToken);
        }

        public void Add(TeamMember teamMember)
        {
            _context.TeamMembers.Add(teamMember);
        }

        public void Remove(TeamMember teamMember)
        {
            _context.TeamMembers.Remove(teamMember);
        }
    }
}
