namespace UserService.Application.Events
{
    public record UserRegisteredEvent
    {
        public int Id { get; init; }
        public string Email { get; init; }
        public string FullName { get; init; }
    }
}
