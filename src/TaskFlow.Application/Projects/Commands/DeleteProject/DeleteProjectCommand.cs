using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;

namespace TaskFlow.Application.Projects.Commands.DeleteProject
{
    public class DeleteProjectCommand : ICommand
    {
        public Guid ProjectId { get; init; }
    }
}
