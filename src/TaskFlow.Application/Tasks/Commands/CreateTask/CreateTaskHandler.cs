using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Common;
using TaskFlow.Domain;
using TaskFlow.Domain.Entities;
using TaskEntity = TaskFlow.Domain.Entities.Task;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Application.Tasks.Commands.CreateTask
{
    public class CreateTaskHandler : ICommandHandler<CreateTaskCommand>
    {
        private readonly ITaskRepository _taskRepo;
        private readonly IProjectRepository _projectRepo;
        private readonly IUnitOfWork _unitOfWork;
        public CreateTaskHandler(ITaskRepository taskRepo, IProjectRepository projectRepo, IUnitOfWork unitOfWork)
        {
            _taskRepo = taskRepo;
            _projectRepo = projectRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(CreateTaskCommand command, CancellationToken cancellationToken = default)
        {
            // 1. Validate Input
            if (string.IsNullOrEmpty(command.Title))
                return Result.Failure("Task title is required");
            
            if (!Enum.TryParse<Priority>(command.Priority, ignoreCase:true , out var priority))
                return Result.Failure("Invalid priority value. Must be Low, Medium, High, or Critical.");

            // 2. Convert primitives to the custom objects
            var projectId = new ProjectId(command.ProjectId);

            // 3. Ensure project exists
            var project = await _projectRepo.GetByIdAsync(projectId, cancellationToken);
            if (project == null)
                return Result.Failure("Project not found.");

            // 4. Create domain entity
            var taskEntity = new TaskEntity(command.Title, priority,projectId);

            // 5. Set optional fields 
            if (!string.IsNullOrEmpty(command.Description))
                taskEntity.ChangeDescription(command.Description);
            if(command.DateRange is not null)
            {
                try
                {
                    var dr = new DateRange(command.DateRange.StartDate, command.DateRange.DueDate);
                    taskEntity.SetDateRange(dr);
                }
                catch (ArgumentException ex)
                {
                    return Result.Failure(ex.Message);
                }
            }

            foreach(var tag in command.Tags)
            {
                try
                {
                    
                    var newTag = new Tag(tag);
                    taskEntity.AddTag(newTag);

                } catch(ArgumentException ex)
                {
                    return Result.Failure($"Invalid tag '{tag}' : '{ex.Message}'");
                }
            }

            // 6. Persist
            _taskRepo.Add(taskEntity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // 7. Return success
            return Result.Success();
        }
    }
}
