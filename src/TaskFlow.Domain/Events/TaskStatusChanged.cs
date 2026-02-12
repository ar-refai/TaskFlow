using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskId = TaskFlow.Domain.ValueObjects.TaskId;

namespace TaskFlow.Domain.Events
{
    public record TaskStatusChanged(TaskId TaskId, TaskStatus PreviousStatud, TaskStatus NewStatus, DateTime OccuredAt): IDomainEvent;
}
