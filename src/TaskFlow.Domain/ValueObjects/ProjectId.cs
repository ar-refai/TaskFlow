using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.ValueObjects
{
    public readonly record struct ProjectId(Guid Value)
    {
        public static ProjectId New() => new(Guid.NewGuid());
        public static ProjectId Empty() => new(Guid.Empty);
    }
}
