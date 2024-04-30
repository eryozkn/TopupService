using Monad;
using TopupService.Domain;

namespace TopupService.Facade.Interfaces
{
    public interface IBeneficiaryFacade
    {
        ValueTask<Either<Beneficiary, Error>> AddBeneficiary(Beneficiary beneficiary, int beneficiaryLimitConfig);
        Either<IEnumerable<Beneficiary>, Error> GetUserActiveBeneficiaries(long userId);
    }
}
