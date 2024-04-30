using Microsoft.EntityFrameworkCore;
using TopupService.DAL.Interfaces;

namespace TopupService.DAL.Implementations
{
    public class BeneficiaryRepository(AppDbContext context) : IBeneficiaryRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<BeneficiaryEntity> AddBeneficiary(BeneficiaryEntity entity)
        {
            _context.Beneficiaries.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public IEnumerable<BeneficiaryEntity> GetActiveBeneficiaries(long userId)
        {
            return _context.Beneficiaries.AsNoTrackingWithIdentityResolution()
                                  .Where(b => b.UserId == userId && b.IsActive);
        }
    }
}
