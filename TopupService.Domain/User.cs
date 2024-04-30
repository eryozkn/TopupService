namespace TopupService.Domain
{
    // A stateless user domain model
    public record User
    {
        public long Id { get; init; }
        public string Username { get; init; } = null!;
        public VerificationStatus Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }

    public enum VerificationStatus
    {
        NotVerified = 0,
        Verified = 1
    }
}
