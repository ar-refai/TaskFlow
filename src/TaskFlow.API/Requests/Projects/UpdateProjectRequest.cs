namespace TaskFlow.API.Requests
{
    public class UpdateProjectRequest
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty; 
        public string? Description { get; init; }
    }
}
