using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Common;
using TaskFlow.Application.Tasks.Commands.CreateTask;
using TaskFlow.Domain;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Repositories.Filters;

namespace TaskFlow.Application.Tasks.Queries.GetTasksWithFilters
{
    public class GetTasksWithFiltersHandler : IQueryHandler<GetTasksWithFiltersQuery, List<TaskResponse>>
    {
        private readonly ITaskRepository _taskRepo;
        private readonly IUnitOfWork _unitOfWork;
        public GetTasksWithFiltersHandler(ITaskRepository taskRepository, IUnitOfWork unitOfWork)
        {
            _taskRepo = taskRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<TaskResponse>>> Handle(GetTasksWithFiltersQuery query, CancellationToken cancellationToken = default)
        {
            var filter = new TaskFilters(query.Status, query.AssigneeId, query.Priority);
            var result = await _taskRepo.GetWithFiltersAsync(filter, cancellationToken);
            var response = result.Select(t => t.ToResponse()).ToList();
            return response;
        }
    }
}
