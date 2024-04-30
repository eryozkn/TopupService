namespace TopupService.Models.Response
{
    public record BeneficiaryResponse
    {
        public long UserId { get; set; }
        public string NickName { get; init; } = null!;
        public string PhoneNumber { get; init; } = null!;
    }
}
