using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Common;
using TaskFlow.Domain;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Application.Projects.Commands.DeleteProject
{
    public class DeleteProjectHandler : ICommandHandler<DeleteProjectCommand>
    {
        private readonly IProjectRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProjectHandler(IProjectRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteProjectCommand command, CancellationToken cancellationToken = default)
        {
            var projectId = new Domain.ValueObjects.ProjectId(command.ProjectId);
            var project = await _repo.GetByIdAsync(projectId, cancellationToken);
            
            if (project is null)
                return Result.Failure("Project not found.");

            _repo.Remove(project);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
