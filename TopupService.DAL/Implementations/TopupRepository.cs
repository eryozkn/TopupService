using Microsoft.EntityFrameworkCore;
using TopupService.DAL.Entities;
using TopupService.DAL.Interfaces;

namespace TopupService.DAL.Implementations
{
    public class TopupRepository(AppDbContext context) : ITopupRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<TopupHistoryEntity> CreateTopup(TopupHistoryEntity entity)
        {
            _context.TopupHistory.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public IEnumerable<TopupHistoryEntity> GetMonthlyUserTopupsToSingleBeneficiary(long userId, long beneficiaryId)
        {
            return _context.TopupHistory.AsNoTrackingWithIdentityResolution()
                    .Where(t => t.UserId == userId && t.BeneficiaryId == beneficiaryId && t.CreatedAt.Month == DateTimeOffset.UtcNow.Month);
        }

        public IEnumerable<TopupHistoryEntity> GetMonthlyUserTopups(long userId)
        {
            return _context.TopupHistory.AsNoTrackingWithIdentityResolution()
                    .Where(t => t.UserId == userId && t.CreatedAt.Month == DateTimeOffset.UtcNow.Month);
        }
    }
}
