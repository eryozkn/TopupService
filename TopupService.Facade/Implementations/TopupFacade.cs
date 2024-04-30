using Microsoft.Extensions.Logging;
using Monad;
using TopupService.DAL.Entities;
using TopupService.DAL.Interfaces;
using TopupService.Domain;
using TopupService.ExternalService;
using TopupService.ExternalService.Models.ServiceRequests;
using TopupService.Facade.Interfaces;

namespace TopupService.Facade.Implementations
{
    public class TopupFacade : ITopupFacade
    {
        private readonly ITopupRepository _repository;
        private readonly ITransactionService _service;
        private readonly ILogger<TopupFacade> _logger;
        private const string _topupCurrency = "AED"; // move to either config or entity
        public TopupFacade(ITopupRepository repository, ITransactionService service, ILogger<TopupFacade> logger)
        {
            _repository = repository;
            _service = service;
            _logger = logger;
        }

        public async ValueTask<Either<Topup, Error>> CreateTopup(Topup topup, IEnumerable<TopupLimit> topupLimits, decimal topupFee)
        {
            try
            {
                // Check limits
                CheckUserTopupLimits(topup, topupLimits);

                // Include fee
                var totalAmount = topup.Amount + topupFee;

                // Check user balance
                var userBalance = await _service.GetUserBalance(topup.UserInfo.Id);

                if (userBalance.Balance == 0)
                {
                    return () => new Error() { Code = ErrorCodes.InvalidUserBalance, Message = "Error in retrieving user balance" };
                }

                if (userBalance.Balance < topup.Amount)
                {
                    return () => new Error() { Code = ErrorCodes.InsufficientUserBalance, Message = "User doesn't have enough balace to topup" };
                }

                // Create transaction and update user balance
                await _service.CreateTransaction(
                    new CreateTransactionRequest()
                    {
                        UserId = topup.UserInfo.Id,
                        Amount = totalAmount,
                        Currency = _topupCurrency,
                        TransactionType = TransactionType.Debit
                    });

                // Add topup history
                var topupRecord = await _repository.CreateTopup(MapToTopupEntity(topup));

                return () => topup;
            } 
            catch (Exception ex) 
            {
                _logger.LogError($"Topup creation failed. Detail: {ex.Message}");
                return () => new Error() { Code = ErrorCodes.InternalServerError, Message = "Error in CreateTopup" };
            }
        }

        private TopupHistoryEntity MapToTopupEntity(Topup topup)
        {
            return new TopupHistoryEntity()
            {
                UserId = topup.UserInfo.Id,
                BeneficiaryId = topup.BeneficiaryId,
                TopupAmount = topup.Amount,
                CreatedAt = DateTimeOffset.UtcNow
            };
        }

        private Func<Error> CheckUserTopupLimits(Topup topup, IEnumerable<TopupLimit> topupLimits)
        {
            var currentTopupAmount = _repository.GetMonthlyUserTopupsToSingleBeneficiary(topup.UserInfo.Id, topup.BeneficiaryId).Sum(t => t.TopupAmount);

            if (topup.UserInfo.Status == VerificationStatus.NotVerified)
            {
                var unverifiedUserLimit = topupLimits.FirstOrDefault(l => l.LimitCode == LimitConstants.UnverifiedUserMonthlyTopupLimit);

                if (unverifiedUserLimit != null && unverifiedUserLimit.Value >= currentTopupAmount)
                {
                    return () => new Error() { Code = ErrorCodes.UserTopupLimitExceed, Message = $"Monthly limit is exceeded by the user" };
                }
            }
            else
            {
                var verifiedUserLimit = topupLimits.FirstOrDefault(l => l.LimitCode == LimitConstants.VerifiedUserMonthlyTopupLimit);

                if (verifiedUserLimit != null && verifiedUserLimit.Value >= currentTopupAmount)
                {
                    return () => new Error() { Code = ErrorCodes.UserTopupLimitExceed, Message = $"Monthly limit is exceeded by the user" };
                }
            }

            var currentMultipleBeneficiaryTopupAmount = _repository.GetMonthlyUserTopups(topup.UserInfo.Id).Sum(t => t.TopupAmount);
            var multipleBeneficiaryMonthlyLimit = topupLimits.FirstOrDefault(l => l.LimitCode == LimitConstants.MultipleBeneficiaryMonthlyLimit);

            if (multipleBeneficiaryMonthlyLimit != null && multipleBeneficiaryMonthlyLimit.Value >= currentMultipleBeneficiaryTopupAmount)
            {
                return () => new Error() { Code = ErrorCodes.UserTopupLimitExceed, Message = $"Monthly total limit to all beneficiaries is exceeded by the user" };
            }

            return () => new Error() { Code = ErrorCodes.InternalServerError, Message = $"Limit check failed" };
        }
    }
}
