using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;

namespace TaskFlow.Application.Tasks.Commands.AddRemoveTagsInTask
{
    public class AddRemoveTagsInTaskCommand : ICommand
    {
        public Guid TaskId { get; init; }
        public List<string> Tags { get; init; } = new();
    }
}
