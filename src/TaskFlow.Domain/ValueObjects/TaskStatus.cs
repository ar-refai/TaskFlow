using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.ValueObjects
{
    public enum TaskStatus
    {
        ToDo,
        InProgress,
        InReview,
        Done,
        Cancelled,
    }
}
