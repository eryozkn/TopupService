namespace TopupService.Domain
{
    public record Topup
    {
        public long Id { get; init; }
        public User UserInfo { get; set; } = null!;
        public long BeneficiaryId { get; init; }
        public decimal Amount { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
