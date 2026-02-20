using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Filters;
using TaskFlow.Domain.ValueObjects;
using Status = TaskFlow.Domain.ValueObjects.TaskStatus;


namespace TaskFlow.Infrastructure.Persistence.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskFlowDbContext _context;

        public TaskRepository(TaskFlowDbContext context)
        {
            _context = context;
        }

        public async Task<List<Domain.Entities.Task>> GetWithFiltersAsync(TaskFilters filters, CancellationToken cancellationToken)
        {
            var query = _context.Tasks.Include(t => t.Comments).AsNoTracking();

            if(!string.IsNullOrEmpty(filters.Status))
            { 
                Enum.TryParse<Status>(filters.Status, true, out var status);
                query.Where(t => t.TaskStatus == status);
            }
            
            if(!string.IsNullOrEmpty(filters.Priority))
            {
                Enum.TryParse<Priority>(filters.Priority, true, out var priority);
                query.Where(t => t.Priority == priority);
            }
            
            if(filters.AssigneeId.HasValue)
            { 
                var assigneeId = new TeamMemberId(filters.AssigneeId.Value);
                query.Where(t => t.AssignedTo == assigneeId);
            }

            return await query.OrderByDescending(t => t.CreatedAt).ToListAsync(cancellationToken);
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
