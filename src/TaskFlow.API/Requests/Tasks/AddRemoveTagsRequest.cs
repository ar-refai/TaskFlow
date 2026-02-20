namespace TaskFlow.API.Requests.Tasks
{
    public class AddRemoveTagsRequest
    {
        public List<string> Tags { get; init; } = new();
    }
}
