
using Monad;
using TopupService.Domain;

namespace TopupService.Facade.Interfaces
{
    public interface ITopupFacade
    {
        ValueTask<Either<Topup, Error>> CreateTopup(Topup topup, IEnumerable<TopupLimit> topupLimits, decimal topupFee);
    }
}
