using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Tasks.Commands.CreateTask;
using TaskStatus = TaskFlow.Domain.ValueObjects.TaskStatus;

namespace TaskFlow.Application.Tasks.Commands.UpdateTask
{
    public class UpdateTaskCommand : ICommand
    {
        public Guid TaskId { get; init; }
        public string Title { get; init; } = string.Empty;
        public string? Description { get; init; }
        public string Priority { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public DateRangeDto? DateRange { get; init; }
    }
}
