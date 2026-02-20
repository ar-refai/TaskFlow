using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Common;
using TaskFlow.Domain;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Application.Tasks.Commands.AddCommentToTask
{
    public class AddCommentToTaskHandler : ICommandHandler<AddCommentToTaskCommand>
    {
        private readonly ITaskRepository _taskRepo;
        private readonly ITeamMemberRepository _memberRepo;
        private readonly IUnitOfWork _unitOfWork;
        public AddCommentToTaskHandler(ITaskRepository taskRepository,ITeamMemberRepository teamMemberRepository, IUnitOfWork unitOfWork)
        {
            _taskRepo = taskRepository;
            _memberRepo = teamMemberRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AddCommentToTaskCommand command, CancellationToken cancellationToken = default)
        {
            var taskId = new TaskId(command.TaskId);
            var task = await _taskRepo.GetByIdAsync(taskId, cancellationToken);
            if (task is null)
                return Result.Failure("Task not found.");

            // validate teamMember as it is not validated else where 
            var memberId = new TeamMemberId(command.TeamMemberId);
            var member = await _memberRepo.GetByIdAsync(memberId, cancellationToken);
            if (member is null)
                return Result.Failure("Team member not found.");

            task.AddComment(command.Content, memberId, taskId);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
