using TopupService.ExternalService.Models.ServiceRequests;
using TopupService.ExternalService.Models.ServiceResponses;

namespace TopupService.ExternalService
{
    public interface ITransactionService
    {
        Task<UserBalanceServiceResponse> GetUserBalance(long userId);
        Task<TransactionResponse?> CreateTransaction(CreateTransactionRequest request);
    }
}
