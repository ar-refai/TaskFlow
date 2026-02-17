using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;
namespace TaskFlow.Application.Projects.Commands.CreateProject
{
    public class CreateProjectCommand : ICommand
    {
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }

    }
}
