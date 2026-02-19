namespace TaskFlow.API.Requests
{
    public class CreateTeamMemberRequest
    {
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
    }
}
