using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Tasks.Commands.CreateTask;

namespace TaskFlow.Application.Tasks.Queries.GetTaskById
{
    public class GetTaskByIdQuery : IQuery<TaskResponse>
    {
        public Guid TaskId { get; init; }
    }
}
