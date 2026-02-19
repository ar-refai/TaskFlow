using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.ValueObjects
{
    public record DateRange
    {
        public DateTime StartDate { get; init; }
        public DateTime DueDate { get; init; }

        public DateRange(DateTime startDate, DateTime dueDate)
        {
            if (dueDate < startDate) throw new ArgumentException("Start date must be before due date.", nameof(startDate));
            StartDate = startDate;
            DueDate = dueDate;
        }

        public int DurationInDays => (DueDate - StartDate).Days;
    }
}
