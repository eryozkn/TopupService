using System.ComponentModel;

namespace TopupService.ExternalService.Models.ServiceRequests
{
    public record CreateTransactionRequest
    {
        public long UserId { get; init; }

        public decimal Amount { get; init; }

        public string Currency { get; init; } = null!;
        public TransactionType TransactionType { get; init; }
    }

    public enum TransactionType
    {
        [Description("Debit")]
        Debit = 1,
        [Description("Credit")]
        Credit = 2,
    }
}
