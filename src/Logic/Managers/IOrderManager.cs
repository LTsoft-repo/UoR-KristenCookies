using JetBrains.Annotations;
using Logic.Infrastructure;

namespace Logic.Managers;

public interface IOrderManager<TOrder>
{
    [ UsedImplicitly ]
    Task<Result<TOrder>> PlaceOrderAsync( TOrder order );

    [ UsedImplicitly ]
    Task<Result<IEnumerable<TOrder>>> GetHistoryAsync();
}