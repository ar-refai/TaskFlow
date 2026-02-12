using TaskStatus = TaskFlow.Domain.ValueObjects.TaskStatus; // using name alias to avoid conflicts
namespace TaskFlow.Domain.Exceptions
{
    public sealed class InvalidStatusTransitionException : DomainException
    {
        public TaskStatus CurrentStatus { get; }
        public TaskStatus AttemptedStatus { get; }
        public InvalidStatusTransitionException(TaskStatus currentStatus, TaskStatus attemptedStatus) : base($"Cannot transition from {currentStatus} to {attemptedStatus}.")
        {
            CurrentStatus = currentStatus;
            AttemptedStatus = attemptedStatus;
        }
    }
}
