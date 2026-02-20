using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Common;
using TaskFlow.Domain;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Application.Tasks.Commands.AddRemoveTagsInTask
{
    public class AddRemoveTagsInTaskHandler : ICommandHandler<AddRemoveTagsInTaskCommand>
    {
        private readonly ITaskRepository _taskRepo;
        private readonly IUnitOfWork _unitOfWork;

        public AddRemoveTagsInTaskHandler(ITaskRepository taskRepository, IUnitOfWork unitOfWork)
        {
            _taskRepo = taskRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AddRemoveTagsInTaskCommand command, CancellationToken cancellationToken = default)
        {
            var taskId = new TaskId(command.TaskId);
            var task = await _taskRepo.GetByIdAsync(taskId, cancellationToken);
            if (task is null) return Result.Failure("Task not found.");

            var tagsToRemove = task.Tags.Where(t => !command.Tags.Contains(t.Value)).ToList();
            var tagsToAdd = command.Tags.Where(val => !task.Tags.Any(t => t.Value == val)).ToList();

            // 2. Remove is safe
            tagsToRemove.ForEach(t => task.RemoveTag(t));

            // 3. Add needs validation (Manual foreach is better here)
            foreach (var tagValue in tagsToAdd)
            {
                try { task.AddTag(new Tag(tagValue)); }
                catch (ArgumentException ex) { return Result.Failure($"Invalid tag '{tagValue}': {ex.Message}"); }

            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();

        }


    }
}
