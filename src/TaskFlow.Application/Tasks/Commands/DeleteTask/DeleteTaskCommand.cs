using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;

namespace TaskFlow.Application.Tasks.Commands.DeleteTask
{
    public class DeleteTaskCommand : ICommand
    {
        public Guid TaskId { get; init; }
    }
}
