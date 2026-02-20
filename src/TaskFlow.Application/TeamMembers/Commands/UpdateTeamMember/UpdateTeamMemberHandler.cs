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

namespace TaskFlow.Application.TeamMembers.Commands.UpdateTeamMember
{
    public class UpdateTeamMemberHandler : ICommandHandler<UpdateTeamMemberCommand>
    {
        private readonly ITeamMemberRepository _memberRepo;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTeamMemberHandler( ITeamMemberRepository memberRepo, IUnitOfWork unitOfWork )
        {
            _memberRepo = memberRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateTeamMemberCommand command, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(command.Name))
                return Result.Failure("Name is required.");

            if (string.IsNullOrWhiteSpace(command.Email))
                return Result.Failure("Email is required.");

            var memberId = new TeamMemberId(command.TeamMemberId);
            var member = await _memberRepo.GetByIdAsync(memberId, cancellationToken);
            if (member is null)
                return Result.Failure("Team member not found.");

            if (member.Email != command.Email)
            {
                // validate email
                Email email;
                try
                {
                    email = new Email(command.Email);
                } catch (Exception ex) 
                {
                    return Result.Failure(ex.Message);
                }

                // check if email already exists
                var existingMember = await _memberRepo.GetByEmailAsync(
                    email,
                    cancellationToken);

                if (existingMember is not null)
                    return Result.Failure("A team member with this email already exists.");
            }

            // change status safe
            try
            {
                member.ChangeEmail(new Email(command.Email));
                member.ChangeName(command.Name);
            }
            catch (ArgumentException ex)
            {
                return Result.Failure(ex.Message);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
