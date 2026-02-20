namespace TaskFlow.API.Requests.Tasks
{
    public class AddCommentRequest
    {
        public Guid AuthorId { get; init; }
        public string Content { get; init; }
    }
}
