using DataModel.Model.Entities;
using JetBrains.Annotations;
using Logic.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Stores.Infrastructure;

namespace Logic.Managers;

[ UsedImplicitly ]
public class OrderManager : IOrderManager<Order>
{
    [ UsedImplicitly ]
    protected readonly IStore<Order> OrderStore;

    public OrderManager( IStore<Order> orderStore )
    {
        OrderStore = orderStore;
    }

    public virtual async Task<Result<Order>> PlaceOrderAsync( Order order )
    {
        var cookieCount = order.Cookies.Sum( cookie => cookie.Quantity );

        if( cookieCount < 12 || cookieCount % 12 != 0 )
        {
            return new()
            {
                Success = false,
                Code = ResultErrorCode.IncompleteDozen,
                Error = "Order must contain complete dozens."
            };
        }

        try
        {
            order = order with { OrderedAtUtc = DateTime.UtcNow };
            var addResult = await OrderStore.AddAsync( order );

            if( !addResult.Success )
            {
                return new()
                {
                    Success = false,
                    Code = ResultErrorCode.StoreError,
                    Error = addResult.Error
                };
            }

            var getResult = await OrderStore.GetAsync( addResult.Data );

            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if( !getResult.Success )
            {
                return new()
                {
                    Success = false,
                    Code = ResultErrorCode.StoreError,
                    Error = getResult.Error
                };
            }

            return new()
            {
                Success = true,
                Data = getResult.Data
            };
        }
        catch( Exception ex )
        {
            return new()
            {
                Success = false,
                Code = ResultErrorCode.StoreError,
                Error = ex.Message
            };
        }
    }

    public virtual async Task<Result<IEnumerable<Order>>> GetHistoryAsync()
    {
        try
        {
            var entities = await OrderStore.GetAll()
                .OrderByDescending( e => e.OrderedAtUtc )
                .ToListAsync();

            return new()
            {
                Success = true,
                // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
                Data = entities ?? Enumerable.Empty<Order>()
            };
        }
        catch( Exception ex )
        {
            return new()
            {
                Success = false,
                Code = ResultErrorCode.StoreError,
                Error = ex.Message
            };
        }
    }
}