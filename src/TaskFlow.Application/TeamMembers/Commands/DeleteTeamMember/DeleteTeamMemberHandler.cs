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

namespace TaskFlow.Application.TeamMembers.Commands.DeleteTeamMember
{
    public class DeleteTeamMemberHandler : ICommandHandler<DeleteTeamMemberCommand>
    {
        private readonly ITeamMemberRepository _memberRepo;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteTeamMemberHandler(ITeamMemberRepository memberRepo , IUnitOfWork unitOfWork)
        {
            _memberRepo = memberRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteTeamMemberCommand command, CancellationToken cancellationToken = default)
        {
            var memberId = new TeamMemberId(command.TeamMemberId);
            var member = await _memberRepo.GetByIdAsync(memberId,cancellationToken);
            if (member is null) return Result.Failure("Team member not found");

            _memberRepo.Remove(member);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
