namespace TopupService.Domain
{
    public record Error
    {
        public string Code { get; init; } = null!;
        public string Message { get; set; } = null!;
    }

    public static class ErrorCodes
    {
        public const string InvalidUser = "INVALID_USER_CREDENTIALS";
        public const string InternalServerError = "INTERNAL_SERVER_ERROR";
        public const string ConfigrationNotFoundError = "CONFIG_NOT_FOUND";
        public const string BeneficiaryLimitExceed = "BENEFICIARY_LIMIT_EXCEED";
        public const string InvalidPhoneNumber = "INVALID_PHONE_NUMBER";
        public const string UserTopupLimitExceed = "USER_TOPUP_LIMIT_EXCEED";
        public const string InsufficientUserBalance = "INSUFFICIENT_USER_BALANCE";
        public const string InvalidUserBalance = "INVALID_USER_BALANCE";
    }
}
