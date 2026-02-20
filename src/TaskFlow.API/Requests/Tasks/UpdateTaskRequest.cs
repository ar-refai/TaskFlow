namespace TaskFlow.API.Requests.Tasks
{
    public class UpdateTaskRequest
    {
        public string Title { get; init; } = string.Empty;
        public string? Description { get; init; }
        public string Priority { get; init; } = string.Empty;
        public DateRangeRequest? DateRange { get; init; }
    }
}
