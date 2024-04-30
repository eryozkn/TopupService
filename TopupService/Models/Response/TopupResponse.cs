namespace TopupService.Models.Response
{
    public record TopupResponse
    {
        public long UserId { get; set; }
        public decimal Amount { get; init; }
        public string BeneficiaryPhoneNumber { get; init; } = null!;
    }
}
