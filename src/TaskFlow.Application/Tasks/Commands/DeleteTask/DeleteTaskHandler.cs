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

namespace TaskFlow.Application.Tasks.Commands.DeleteTask
{
    public class DeleteTaskHandler : ICommandHandler<DeleteTaskCommand>
    {
        private readonly ITaskRepository _taskRepo;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTaskHandler(ITaskRepository taskRepo, IUnitOfWork unitOfWork)
        {
            _taskRepo = taskRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteTaskCommand command, CancellationToken cancellationToken = default)
        {
            var taskId = new TaskId(command.TaskId);
            var task = await _taskRepo.GetByIdAsync(taskId,cancellationToken);
            if (task is null)
                return Result.Failure("Task not found.");
            _taskRepo.remove(task);
            await _unitOfWork.SaveChangesAsync(cancellationToken);   
            return Result.Success();
        }
    }
}
