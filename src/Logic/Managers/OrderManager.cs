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
    protected readonly ICustomerManager<Customer> CustomerManager;

    [ UsedImplicitly ]
    protected readonly IStore<Order> OrderStore;

    public OrderManager( IStore<Order> orderStore, ICustomerManager<Customer> customerManager )
    {
        OrderStore = orderStore;
        CustomerManager = customerManager;
    }

    public virtual async Task<Result<Order>> PlaceOrderAsync( Order order )
    {
        var canAdd = await CanAddAsync( order );

        if( !string.IsNullOrEmpty( canAdd ) )
        {
            return new()
            {
                Success = false,
                Code = ResultErrorCode.Validation,
                Error = canAdd
            };
        }

        var customerAddResult = await CustomerManager.FindByEmailAsync( order.Customer.Email );

        if( customerAddResult.Success )
        {
            order.Customer = customerAddResult.Data!;
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

    protected virtual string ValidateEntity( Order entity )
    {
        if( string.IsNullOrWhiteSpace( entity.Customer.Email ) )
        {
            return "You must provide the customer e-mail.";
        }

        var cookieCount = entity.Cookies.Sum( cookie => cookie.Quantity );

        if( cookieCount < 12 || cookieCount % 12 != 0 )
        {
            return "The Order must contain complete dozens.";
        }

        return "";
    }

    protected virtual async Task<string> CanAddAsync( Order entity )
    {
        var isValidInput = ValidateEntity( entity );

        if( isValidInput != "" )
        {
            return isValidInput;
        }

        var result = await CustomerManager.FindByEmailAsync( entity.Customer.Email );

        if( !result.Success && string.IsNullOrWhiteSpace( entity.Customer.Name ) )
        {
            return "The Customer does not exists. Please provide a name.";
        }

        return "";
    }
}