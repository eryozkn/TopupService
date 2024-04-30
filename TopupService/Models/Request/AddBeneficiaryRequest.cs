using FluentValidation;

namespace TopupService.Models.Request
{
    public record AddBeneficiaryRequest
    {
        public long UserId { get; init; }

        public string NickName { get; init; } = null!;

        public string PhoneNumber { get; init; } = null!;
    }

    public class AddBeneficiaryRequestValidator : AbstractValidator<AddBeneficiaryRequest>
    {
        public AddBeneficiaryRequestValidator()
        {
            RuleFor(x => x.UserId).NotNull().GreaterThan(0);
            RuleFor(x => x.NickName).NotNull().MaximumLength(20);
            RuleFor(x => x.PhoneNumber).NotNull();
        }
    }
}
