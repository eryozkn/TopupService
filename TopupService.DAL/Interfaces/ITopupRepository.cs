using TopupService.DAL.Entities;

namespace TopupService.DAL.Interfaces
{
    public interface ITopupRepository
    {
        Task<TopupHistoryEntity> CreateTopup(TopupHistoryEntity entity);
        IEnumerable<TopupHistoryEntity> GetMonthlyUserTopupsToSingleBeneficiary(long userId, long beneficiaryId);
        IEnumerable<TopupHistoryEntity> GetMonthlyUserTopups(long userId);
    }
}
