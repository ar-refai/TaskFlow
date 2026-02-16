using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Infrastructure.Persistence.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly TaskFlowDbContext _context;
        public ProjectRepository(TaskFlowDbContext context)
        {
            _context = context;
        }


        public async Task<List<Project>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Projects.AsNoTracking().OrderByDescending(p => p.Name).ToListAsync(cancellationToken);
        }

        public async Task<Project?> GetByIdAsync(ProjectId projectId, CancellationToken cancellationToken = default)
        {
            return await _context.Projects.FirstOrDefaultAsync(p => p.Id == projectId, cancellationToken);
        }


        public void Add(Project project)
        {
            _context.Projects.Add(project);
        }

        public void Remove(Project project)
        {
            _context.Projects.Remove(project);
        }
    }
}
