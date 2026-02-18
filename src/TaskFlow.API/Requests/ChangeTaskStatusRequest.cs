namespace TaskFlow.API.Requests
{
    public class ChangeTaskStatusRequest
    {
        public string NewStatus { get; init; } = string.Empty;
    }
}
