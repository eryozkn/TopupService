using System.Net.Sockets;

namespace TopupService.DAL.Interfaces
{
    public interface IBeneficiaryRepository
    {
        Task<BeneficiaryEntity> AddBeneficiary(BeneficiaryEntity entity);
        IEnumerable<BeneficiaryEntity> GetActiveBeneficiaries(long userId);
    }
}
