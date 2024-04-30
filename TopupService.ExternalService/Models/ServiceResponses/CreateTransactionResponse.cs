namespace TopupService.ExternalService.Models.ServiceResponses
{
    public record TransactionResponse
    {
        public Guid TransactionReference { get; init; }

        public long UserId { get; init; }

        public decimal Amount { get; init; }

        public string Currency { get; init; } = null!;

        public DateTimeOffset? CreatedAt { get; set; }
    }
}
