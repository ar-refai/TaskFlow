using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Common;
using TaskFlow.Domain;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.ValueObjects;
namespace TaskFlow.Application.TeamMembers.Commands.CreateTeamMember
{
    public class CreateTeamMemberHandler : ICommandHandler<CreateTeamMemberCommand>
    {
        private readonly ITeamMemberRepository _teamMemberRepo;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTeamMemberHandler(ITeamMemberRepository teamMemberRepository, IUnitOfWork unitOfWork)
        {
            _teamMemberRepo = teamMemberRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(CreateTeamMemberCommand command, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(command.Name)) return Result.Failure("Name is required.");
            if (string.IsNullOrEmpty(command.Email)) return Result.Failure("Email is required.");

            var email = new Email(command.Email);
            var existingMember = await _teamMemberRepo.GetByEmailAsync(email,cancellationToken);
            if (existingMember is not null) return Result.Failure("A team member with same email exists.");


            TeamMember teamMember;
            try
            {
                teamMember = new TeamMember(command.Name, email);
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message);
            }
            
            _teamMemberRepo.Add(teamMember);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return Result.Success();
        }
    }
}
