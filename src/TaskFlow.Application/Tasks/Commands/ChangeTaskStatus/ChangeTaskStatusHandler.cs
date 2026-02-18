using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Common;
using TaskFlow.Domain;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.ValueObjects;
using TaskStatusEnum = TaskFlow.Domain.ValueObjects.TaskStatus;

namespace TaskFlow.Application.Tasks.Commands.ChangeTaskStatus
{
    public class ChangeTaskStatusHandler : ICommandHandler<ChangeTaskStatusCommand>
    {
        private readonly ITaskRepository _taskRepo;
        private readonly IUnitOfWork _unitOfWork;
        public ChangeTaskStatusHandler (ITaskRepository taskRepository, IUnitOfWork unitOfWork)
        {
            _taskRepo = taskRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(ChangeTaskStatusCommand command, CancellationToken cancellationToken = default)
        {
            // validate new status
            if (!Enum.TryParse<TaskStatusEnum>(command.newStatus, ignoreCase: true, out var status)) return Result.Failure("New status is not valid.");

            // get the task
            var taskId = new TaskId(command.TaskId);
            var task = await _taskRepo.GetByIdAsync(taskId,cancellationToken);
            if (task is null) return Result.Failure("Task not found.");
        
            // change status 
            task.ChangeStatus(status);
            
            // persist
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return Result.Success();
        }
    }
}
