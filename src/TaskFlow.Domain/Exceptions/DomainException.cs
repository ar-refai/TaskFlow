using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Exceptions
{
    public abstract class DomainException : Exception
    {
        protected DomainException(string msg) : base(msg) { }
        protected DomainException(string msg, Exception innerException) : base(msg, innerException) { }
    }

}
