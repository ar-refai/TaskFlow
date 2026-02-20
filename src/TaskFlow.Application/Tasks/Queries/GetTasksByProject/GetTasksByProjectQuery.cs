using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Projects.Commands.CreateProject;
using TaskFlow.Application.Tasks.Commands.CreateTask;

namespace TaskFlow.Application.Tasks.Queries.GetTasksByProject
{
    public class GetTasksByProjectQuery : IQuery<List<TaskResponse>>
    {
        public Guid ProjectId { get; init; }
    }
}
