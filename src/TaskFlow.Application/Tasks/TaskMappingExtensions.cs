using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Tasks.Commands.CreateTask;
using TaskEntity = TaskFlow.Domain.Entities.Task;
namespace TaskFlow.Application.Tasks
{
    public static class TaskMappingExtensions
    {
        public static TaskResponse ToResponse(this TaskEntity task)
        {
            return new TaskResponse
            {
                Id = task.Id.Value,
                ProjectId = task.ProjectId.Value,
                Title = task.Title,
                Description = task.Description,
                Status = task.TaskStatus.ToString(),
                Priority = task.Priority.ToString(),
                DateRange = task.DateRange is not null?
                new DateRangeDto
                {
                    StartDate =  task.DateRange.Value.StartDate,
                    DueDate = task.DateRange.Value.DueDate
                } : null,
                AssignedTo = task.AssignedTo?.Value,
                Tags = task.Tags.Select(t => t.Value).ToList(),
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt
            };
        }
    }
}
