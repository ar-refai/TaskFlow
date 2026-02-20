using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;

namespace TaskFlow.Application.Tasks.Commands.AddCommentToTask
{
    public class AddCommentToTaskCommand : ICommand
    {
        public Guid TaskId { get; init; }
        public Guid TeamMemberId { get; init; }
        public string Content { get; init; } = string.Empty;
    }
}
