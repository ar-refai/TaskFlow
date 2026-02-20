using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Abstractions;
using TaskFlow.Application.Tasks.Commands.CreateTask;

namespace TaskFlow.Application.Tasks.Queries.GetTasksWithFilters
{
    public class GetTasksWithFiltersQuery : IQuery<List<TaskResponse>>
    {
        public string? Status { get; init; }
        public Guid? AssigneeId { get; init; }
        public string? Priority { get; init; }
    }
}
