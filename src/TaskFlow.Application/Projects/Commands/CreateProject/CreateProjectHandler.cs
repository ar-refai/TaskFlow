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

namespace TaskFlow.Application.Projects.Commands.CreateProject
{
    public class CreateProjectHandler : ICommandHandler<CreateProjectCommand>
    {
        private readonly IProjectRepository _projectRepo;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProjectHandler (IProjectRepository projectRepo, IUnitOfWork unitOfWork)
        {
            _projectRepo = projectRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(CreateProjectCommand command, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(command.Name)) 
                return Result.Failure("Project name is required");
            var projectEntity = new Project(command.Name, command.Description);
            _projectRepo.Add(projectEntity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
