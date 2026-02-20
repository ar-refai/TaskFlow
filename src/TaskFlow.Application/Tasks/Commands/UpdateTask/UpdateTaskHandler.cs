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
using TaskFlow.Domain.ValueObjects;
using TaskStatus = TaskFlow.Domain.ValueObjects.TaskStatus;

namespace TaskFlow.Application.Tasks.Commands.UpdateTask
{
    public class UpdateTaskHandler : ICommandHandler<UpdateTaskCommand>
    {
        private readonly ITaskRepository _taskRepo;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTaskHandler(ITaskRepository taskRepo, IUnitOfWork unitOfWork)
        {
            _taskRepo = taskRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateTaskCommand command, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(command.Title))
                return Result.Failure("Title is required.");
            if (Enum.TryParse<Priority>(command.Priority, true, out Priority priority))
                return Result.Failure("Priority is not valid.");
            if (Enum.TryParse<TaskStatus>(command.Status,true,out TaskStatus status))
                return Result.Failure("Status is not valid.");


            var taskId = new TaskId(command.TaskId);
            var task = await _taskRepo.GetByIdAsync(taskId, cancellationToken);
            if (task is null)
                return Result.Failure("Task not found.");
                    
                    
            task.ChangeTitle(command.Title);
            if(!string.IsNullOrEmpty(command.Description))
                task.ChangeDescription(command.Description);
            task.ChangePriority(priority);
            task.ChangeStatus(status);
            if(command.DateRange is not null)
            {
                try 
                { 
                    var dateRange = new DateRange(
                    command.DateRange.StartDate,
                    command.DateRange.DueDate);
                    task.SetDateRange(dateRange);
                }
                catch (ArgumentException ex)
                {
                    return Result.Failure(ex.Message);
                }
            }

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return Result.Success();

        }
    }
}
