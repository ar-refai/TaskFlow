using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;

namespace TaskFlow.Application.Tasks.Commands.CreateTask
{
    public class CreateTaskCommand : ICommand
    {
        public Guid ProjectId { get; init; }
        public string Title { get; init; } = string.Empty;
        public string? Description { get; init; }
        public string Priority { get; init; } = string.Empty;
        public DateRangeDto? DateRange { get; init; }
        public List<string> Tags { get; init; } = new();
    }
}
