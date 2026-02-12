using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Domain.Events
{
    public sealed record TaskCompleted(TaskId TaskId, DateTime CompletedAt,DateTime OccuredAt) : IDomainEvent;
}
