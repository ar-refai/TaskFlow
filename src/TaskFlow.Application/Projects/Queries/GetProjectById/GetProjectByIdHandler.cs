using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Common;
using TaskFlow.Application.Projects.Commands.CreateProject;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Application.Projects.Queries.GetProjectById
{
    public class GetProjectByIdHandler : IQueryHandler<GetProjectByIdQuery, ProjectResponse>
    {
        private readonly IProjectRepository _projectRepo;
        public GetProjectByIdHandler(IProjectRepository projectRepository)
        {
            _projectRepo = projectRepository;
        }

        public async Task<Result<ProjectResponse>> Handle(GetProjectByIdQuery query, CancellationToken cancellationToken = default)
        {
            var projectId = new ProjectId(query.ProjectId);
            var projectEntity = await _projectRepo.GetByIdAsync(projectId, cancellationToken);
            if (projectEntity == null)
                return Result.Failure<ProjectResponse>("Project not found");
            var response = projectEntity.ToResponse();

            return Result.Success(response);
        }
    }
}
