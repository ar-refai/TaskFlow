using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskId = TaskFlow.Domain.ValueObjects.TaskId;
using TaskStatus = TaskFlow.Domain.ValueObjects.TaskStatus;
namespace TaskFlow.Domain.Events
{
    public record TaskStatusChanged(TaskId TaskId, TaskStatus PreviousStatus, TaskStatus NewStatus, DateTime OccuredAt): IDomainEvent;
}
