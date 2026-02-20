using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Common;
using TaskFlow.Application.Projects.Commands.CreateProject;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Application.Projects.Queries.GetAllProjects
{
    public class GetAllProjectsHandler : IQueryHandler<GetAllProjectsQuery, List<ProjectResponse>>
    {
        private readonly IProjectRepository _repo;
        public GetAllProjectsHandler (IProjectRepository repo)
        {
            _repo = repo;
        }

        public async Task<Result<List<ProjectResponse>>> Handle(GetAllProjectsQuery query, CancellationToken cancellationToken = default)
        { 
            var entities = await _repo.GetAllAsync(cancellationToken);
            List<ProjectResponse> result = entities.Select(e => e.ToResponse()).ToList();
            return Result.Success(result);
        }
    }
}
