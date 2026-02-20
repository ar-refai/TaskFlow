using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Projects.Commands.CreateProject;

namespace TaskFlow.Application.Projects.Queries.GetProjectById
{
    public class GetProjectByIdQuery : IQuery<ProjectResponse>
    {
        public Guid ProjectId { get; init; }
    }
}
