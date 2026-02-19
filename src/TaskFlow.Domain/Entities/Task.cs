using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Events;
using TaskFlow.Domain.Exceptions;
using TaskFlow.Domain.ValueObjects;
using TaskStatus = TaskFlow.Domain.ValueObjects.TaskStatus;

namespace TaskFlow.Domain.Entities
{
    public class Task : Entity<TaskId>
    {
        // Backing fields
        private string _title = string.Empty;
        private string? _description;
        private TaskStatus _taskStatus;
        private Priority _priority;
        private DateRange? _dateRange;
        private TeamMemberId? _assignedTo;
        private readonly List<Tag> _tags = new();
        private readonly List<Comment> _comments = new();

        // Public read-only properties
        public override TaskId Id { get; protected set; }
        public ProjectId ProjectId { get; protected set;}


        public string Title => _title;
        public string? Description => _description;
        public TaskStatus TaskStatus => _taskStatus;
        public Priority Priority => _priority;
        public DateRange? DateRange => _dateRange;
        public TeamMemberId? AssignedTo => _assignedTo;

        public IReadOnlyList<Tag> Tags => _tags.AsReadOnly();
        public IReadOnlyList<Comment> Comments => _comments.AsReadOnly();

        // Audit Fields
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        // EF Core Constructor
        private Task() { }

        // Domain Constructor
        public Task(string title, Priority priority,ProjectId projectId)
        {
            if (string.IsNullOrEmpty(title)) throw new ArgumentException("Task title cannot be empty", nameof(title));
            if(title.Length > 200) throw new ArgumentException("Task title cannot exceed 200 characters",nameof(title));

            Id = TaskId.New();
            ProjectId = projectId;
            _title = title;
            _priority = priority;
            _taskStatus = TaskStatus.ToDo; // New tasks are always a TODO
            CreatedAt = DateTime.UtcNow;
        }

        // Behavioral methods 

        public void ChangeTitle(string newTitle)
        {
            if (string.IsNullOrEmpty(newTitle)) throw new ArgumentException("Task title cannot be empty", nameof(newTitle));
            if (newTitle.Length > 200) throw new ArgumentException("Task title cannot exceed 200 characters", nameof(newTitle));
            _title = newTitle;
        }

        public void ChangeDescription(string newDescription) { _description = newDescription; }

        public void ChangePriority(Priority priority) { _priority = priority; }

        public void SetDateRange(DateRange dateRange) {_dateRange = dateRange; }

        public void ClearDateRange() { _dateRange = null; }
    
        public void Assign(TeamMemberId teamMember) 
        { 
            _assignedTo = teamMember;
            AddDomainEvent(new TaskAssigned(
                TaskId: Id,
                AssignedTo: teamMember,
                OccuredAt: DateTime.UtcNow));
        }

        public void UnAssign() { _assignedTo = null; }

        public void ChangeStatus(TaskStatus newStatus)
        {
            // valid transitions table
            var validTansitions = new Dictionary<TaskStatus, TaskStatus[]>
            {
                // ToDo, InProgress, InReview, Done Cancelled
                [TaskStatus.ToDo] = new[] { TaskStatus.InProgress, TaskStatus.Cancelled },
                [TaskStatus.InProgress] = new[] { TaskStatus.InReview, TaskStatus.Cancelled },
                [TaskStatus.InReview] = new[] { TaskStatus.InProgress, TaskStatus.Done },
                [TaskStatus.Done] = Array.Empty<TaskStatus>(),  // terminal state
                [TaskStatus.Cancelled] = Array.Empty<TaskStatus>() // terminal 
            };

            if (!validTansitions[_taskStatus].Contains(newStatus)) 
                throw new InvalidStatusTransitionException(_taskStatus, newStatus);

            var previousStatus = _taskStatus;
            _taskStatus = newStatus;

            AddDomainEvent(new TaskStatusChanged(
                TaskId: Id, 
                PreviousStatus:previousStatus,
                NewStatus: newStatus,
                OccuredAt: DateTime.UtcNow
                ));
        }

        public void Completed() 
        {
            if (_taskStatus == TaskStatus.Done) throw new TaskAlreadyCompletedException(Id);
            if (_taskStatus != TaskStatus.InReview) throw new InvalidStatusTransitionException(_taskStatus, TaskStatus.Done);
            _taskStatus = TaskStatus.Done;
            AddDomainEvent(new TaskCompleted(
                TaskId: Id,
                CompletedAt: DateTime.UtcNow,
                OccuredAt: DateTime.UtcNow)
                );
        }

        public void AddTag(Tag newTag)
        {
            if (_tags.Contains(newTag)) return; // ignore
            _tags.Add(newTag);
        }

        public void RemoveTag(Tag tag) { _tags.Remove(tag); }

        public void AddComment(Comment comment)
        {
            if (_taskStatus == TaskStatus.Done || _taskStatus == TaskStatus.Cancelled)
                throw new InvalidOperationException("Cannot add comments to completed or cancelled tasks");
            _comments.Add(comment);
        }
    }
}
