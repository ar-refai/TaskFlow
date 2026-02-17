using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.Projects.Commands.CreateProject
{
    public class ProjectResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }

        public DateTime CreatedAt { get; init; }
        public DateTime? UpdattedAt { get; init; }

    }
}
