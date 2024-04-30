namespace TopupService.Models.Response
{
    public record ErrorResponse
    {
        public string Code { get; init; } = null!;
        public string Message { get; init; } = null!;
    }
}
