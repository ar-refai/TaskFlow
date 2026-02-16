using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Infrastructure.Persistence.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskFlowDbContext _context;

        public TaskRepository(TaskFlowDbContext context)
        {
            _context = context;
        }

        public async Task<Domain.Entities.Task?> GetByIdAsync(TaskId taskId, CancellationToken cancellationToken = default)
        {
            return await _context.Tasks
                .Include(t => t.Comments)
                .FirstOrDefaultAsync(t => t.Id == taskId, cancellationToken);
        }

        public async Task<List<Domain.Entities.Task>> GetByProjectAsync(ProjectId projectId, CancellationToken cancellationToken = default)
        {
            return await _context.Tasks
                .AsNoTracking()
                .Where(t => t.ProjectId == projectId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Domain.Entities.Task>> GetByAsigneeAsync(TeamMemberId teamMemberId, CancellationToken cancellationToken = default)
        {
            return await _context.Tasks
                .AsNoTracking()
                .Where(t => t.AssignedTo == teamMemberId)
                .OrderByDescending( t=> t.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public void Add(Domain.Entities.Task task)
        {
            _context.Tasks.Add(task);
        }
  
        public void remove(Domain.Entities.Task task)
        {
            _context.Tasks.Remove(task);
        }
    }
}
