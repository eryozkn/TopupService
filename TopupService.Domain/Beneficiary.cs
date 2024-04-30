namespace TopupService.Domain
{
    public record Beneficiary
    {
        public long UserId { get; set; }
        public string PhoneNumber { get; set; } = null!;
        public string NickName { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
