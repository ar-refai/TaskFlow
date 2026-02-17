namespace TaskFlow.API.Controllers
{
    public class CreateProjectRequest
    {
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
    }
}
