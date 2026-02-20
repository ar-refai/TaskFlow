using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Common;
using TaskFlow.Application.Projects.Commands.CreateProject;
using TaskFlow.Application.Tasks.Commands.CreateTask;
using TaskFlow.Domain;
using TaskFlow.Domain.Repositories;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Application.Tasks.Queries.GetTasksByProject
{
    public class GetTasksByProjectHandler : IQueryHandler<GetTasksByProjectQuery, List<TaskResponse>>
    {
        private readonly ITaskRepository _taskRepo;
        private readonly IProjectRepository _projectRepo;

        public GetTasksByProjectHandler(IProjectRepository projectRepository, ITaskRepository taskRepository)
        {
            _taskRepo = taskRepository;
            _projectRepo = projectRepository;
        }

        public async Task<Result<List<TaskResponse>>> Handle(GetTasksByProjectQuery query, CancellationToken cancellationToken = default)
        {
            // validate project
            var projectId = new ProjectId(query.ProjectId);
            var project = await _projectRepo.GetByIdAsync(projectId, cancellationToken);
            if (project is null)
                return Result.Failure<List<TaskResponse>>("Project not found");

            // fetch the tasks 
            var tasks = await _taskRepo.GetByProjectAsync(projectId, cancellationToken);
            if (!tasks.Any())
                return Result.Failure<List<TaskResponse>>("Project has no tasks.");

            var result = tasks.Select(t => t.ToResponse()).ToList();

            return Result.Success(result);
        }

    }
}
