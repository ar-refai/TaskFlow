using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;

namespace TaskFlow.Application.Tasks.Commands.ChangeTaskStatus
{
    public class ChangeTaskStatusCommand : ICommand
    {
        public Guid TaskId { get; init; }
        public string newStatus { get; init; } = string.Empty;
    }
}
