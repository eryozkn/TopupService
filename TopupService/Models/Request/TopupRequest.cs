using FluentValidation;

namespace TopupService.Models.Request
{
    public record TopupRequest
    {
        public long UserId { get; init; }

        public int BeneficiaryId { get; init; }
        public decimal Amount { get; set; }
    }

    public class TopupRequestValidator : AbstractValidator<TopupRequest>
    {
        public TopupRequestValidator()
        {
            RuleFor(x => x.UserId).NotNull().GreaterThan(0);
            RuleFor(x => x.BeneficiaryId).NotNull();
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }
}
