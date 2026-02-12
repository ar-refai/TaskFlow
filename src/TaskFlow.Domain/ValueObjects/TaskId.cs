
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.ValueObjects
{
    public readonly record struct TaskId(Guid Value)
    {
        public static TaskId New() => new(Guid.NewGuid());
        public static TaskId Empty => new(Guid.Empty);
    }
}
