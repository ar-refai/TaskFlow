using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskFlow.Domain.ValueObjects;
namespace TaskFlow.Domain.Repositories.Filters
{
    public record TaskFilters(string? Status, Guid? AssigneeId,string? Priority);
   
}
