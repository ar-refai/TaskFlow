namespace TaskFlow.API.Requests.TeamMembers
{
    public class CreateTeamMemberRequest
    {
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
    }
}
