using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Common;
using TaskFlow.Application.Tasks.Commands.CreateTask;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Application.Tasks.Queries
{
    public class GetTaskByIdHandler : IQueryHandler<GetTaskByIdQuery, TaskResponse>
    {
        private readonly ITaskRepository _taskRepo;
        public GetTaskByIdHandler(ITaskRepository taskRepo)
        {
            _taskRepo = taskRepo;
        }
        public async Task<Result<TaskResponse>> Handle(GetTaskByIdQuery query, CancellationToken cancellationToken = default)
        {
            var taskId = new TaskId(query.TaskId);
            var task = await _taskRepo.GetByIdAsync(taskId, cancellationToken);
            if (task == null)
                return Result<TaskResponse>.Failure<TaskResponse>("Task not found.");
            var response = new TaskResponse
            {
                Id = task.Id.Value,
                ProjectId = task.ProjectId.Value,
                Title = task.Title,
                Description = task.Description,
                Status = task.TaskStatus.ToString(),
                Priority = task.Priority.ToString(),
                DateRange = task.DateRange is not null ?
                new DateRangeDto
                {
                    StartDate = task.DateRange.Value.StartDate,
                    DueDate = task.DateRange.Value.DueDate
                }
                : null,
                AssignedTo = task.AssignedTo?.Value,
                Tags = task.Tags.Select(t => t.Value).ToList(),
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };
          
            return Result<TaskResponse>.Success(response);
        }
    }
}
