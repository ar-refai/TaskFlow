using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.ValueObjects
{
    public readonly record struct UserId(Guid Value)
    {
        public static UserId New() => new UserId(Guid.NewGuid());
        public static UserId Empty => new UserId(Guid.Empty);
    }
}
