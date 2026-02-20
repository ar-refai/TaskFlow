using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Repositories.Filters;
using TaskFlow.Domain.ValueObjects;
using TaskEntity = TaskFlow.Domain.Entities.Task;


namespace TaskFlow.Domain.Repositories
{
    public interface ITaskRepository
    {
        Task<TaskEntity?> GetByIdAsync(TaskId taskId, CancellationToken cancellationToken = default);
        Task<List<TaskEntity>> GetByProjectAsync(ProjectId projectId, CancellationToken cancellationToken = default);
        Task<List<TaskEntity>> GetByAsigneeAsync(TeamMemberId teamMemberId, CancellationToken cancellationToken = default);
        Task<List<Domain.Entities.Task>> GetWithFiltersAsync(TaskFilters filters, CancellationToken cancellationToken);

        void Add(TaskEntity task);
        void remove(TaskEntity task);
    }
}
