namespace TopupService.DAL
{
    public record BeneficiaryEntity
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string NickName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
