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

namespace TaskFlow.Application.Tasks.Queries.GetTaskById
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
                return Result.Failure<TaskResponse>("Task not found.");
            var response = task.ToResponse();
          
            return Result.Success(response);
        }
    }
}
