namespace TopupService.Domain
{
    public record TopupOption
    {
        public string Option { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
    }
}
