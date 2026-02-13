using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.ValueObjects;

namespace TaskFlow.Domain.Entities
{
    public class Comment : Entity<CommentId>
    {
        public override CommentId Id { get; protected set; }
    }
}
