using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Domain.Exceptions
{
    public sealed class TaskAlreadyCompletedException : DomainException
    {
        public TaskId TaskId { get; }
        public TaskAlreadyCompletedException(TaskId taskId) :  base($"Task {taskId.Value} is already completed.")
        {
            TaskId = taskId;
        }
    }
}
