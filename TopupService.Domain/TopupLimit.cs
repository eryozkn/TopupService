namespace TopupService.Domain
{
    public class TopupLimit
    {
        public string LimitCode { get; set; } = null!;
        public decimal Value { get; set; }
    }

    public static class LimitConstants
    {
        public const string VerifiedUserMonthlyTopupLimit = "VerifiedUserMonthlyTopupLimit";
        public const string UnverifiedUserMonthlyTopupLimit = "UnverifiedUserMonthlyTopupLimit";
        public const string MultipleBeneficiaryMonthlyLimit = "MultipleBeneficiaryMonthlyLimit";
    }
}
