using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskFlow.Application.Common;
namespace TaskFlow.Application.Abstractions
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        Task<Result> Handle(TCommand command, CancellationToken cancellationToken = default);
    }
}
