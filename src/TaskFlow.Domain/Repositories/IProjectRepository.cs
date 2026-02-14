using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Domain.Repositories
{
    public interface IProjectRepository
    {
        Task<Project?> GetByIdAsync(ProjectId projectId,CancellationToken cancellationToken = default);
        Task<List<Project>> GetAllAsync(CancellationToken cancellationToken = default);

        void Add(Project project);
        void Remove(Project project);
    }
}
