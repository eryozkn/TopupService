namespace TopupService.ExternalService.Models.ServiceResponses
{
    public record UserBalanceServiceResponse
    {
        public long UserId { get; init; }

        public decimal Balance { get; init; }

        public string Currency { get; init; } = null!;
    }
}
