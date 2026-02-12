using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.ValueObjects
{
    public readonly record struct TeamMemberId(Guid Value)
    {
        public static TeamMemberId New() => new(Guid.NewGuid());
        public static TeamMemberId Empty => new(Guid.Empty);
    }
}
