namespace TopupService.DAL.Entities
{
    public record TopupHistoryEntity
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long BeneficiaryId { get; set; }
        public decimal TopupAmount { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
