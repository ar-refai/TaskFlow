using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.ValueObjects
{
    public readonly record struct CommentId(Guid Value)
    {
        public static CommentId New() => new(Guid.NewGuid());
        public static CommentId Empty => new(Guid.Empty);
    }
}
