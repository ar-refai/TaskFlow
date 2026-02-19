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

namespace TaskFlow.Application.Projects.Commands.UpdateProject
{
    public class UpdateProjectHandler : ICommandHandler<UpdateProjectCommand>
    {
        private readonly IProjectRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProjectHandler(IProjectRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateProjectCommand command, CancellationToken cancellationToken = default)
        {
            var ProjectId = new ProjectId(command.Id);
            var entity = await _repo.GetByIdAsync(ProjectId, cancellationToken);
            if (entity is null) return Result.Failure("Project not found.");

            entity.ChangeName(command.Name);
            entity.ChangeDescription(command.Description);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
