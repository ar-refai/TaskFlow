using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Projects.Commands.CreateProject;

namespace TaskFlow.Application.Projects.Queries.GetAllProjects
{
    public class GetAllProjectsQuery : IQuery<List<ProjectResponse>>
    {
        // no parameters needed : returns all projects
    }
}
