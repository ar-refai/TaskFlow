namespace TaskFlow.API.Requests.Tasks
{
    public class ChangeTaskStatusRequest
    {
        public string NewStatus { get; init; } = string.Empty;
    }
}
