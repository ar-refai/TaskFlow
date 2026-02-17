using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Projects.Commands.CreateProject;
using TaskFlow.Domain.Entities;
using ProjectEntity = TaskFlow.Domain.Entities.Project;


namespace TaskFlow.Application.Projects
{
    public  static class ProjectMappingExtensions
    {
        public static ProjectResponse ToResponse(this ProjectEntity project)
        {
            return new ProjectResponse
            {
                Id = project.Id.Value,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt,
                UpdattedAt = project.UpdatedAt
            };
        }
    }
}
