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

namespace TaskFlow.Application.Tasks.Commands.AssignTask
{
    public class AssignTaskHandler : ICommandHandler<AssignTaskCommand>
    {
        private readonly ITaskRepository _taskRepo;
        private readonly ITeamMemberRepository _memberRepo;
        private readonly IUnitOfWork _unitOfWork;
        public AssignTaskHandler(ITaskRepository taskRepo, ITeamMemberRepository memberRepo, IUnitOfWork unitOfWork)
        {
            _taskRepo = taskRepo;
            _memberRepo = memberRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AssignTaskCommand command, CancellationToken cancellationToken = default)
        {
            // get the task
            var taskId = new TaskId(command.TaskId);
            var task = await _taskRepo.GetByIdAsync(taskId, cancellationToken);
            if (task is null) return Result.Failure("Task not found.");
            // verify the member
            var memberId = new TeamMemberId(command.TeamMemberId);
            var member = await _memberRepo.GetByIdAsync(memberId, cancellationToken);
            if (member is null) return Result.Failure("Team member not found.");
            task.Assign(memberId);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
