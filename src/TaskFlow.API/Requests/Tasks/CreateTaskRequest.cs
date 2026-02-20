namespace TaskFlow.API.Requests.Tasks
{
    public class CreateTaskRequest
    {
        public string Title { get; init; } = string.Empty;
        public string? Description { get; init; }
        public string Priority { get; init; } = string.Empty;
        public DateRangeRequest? DateRange { get; init; }
        public List<string> Tags { get; init; } = new();


    }
    public class DateRangeRequest
    {
        public DateTime StartDate { get; init; }
        public DateTime DueDate { get; init; }
    }
}
