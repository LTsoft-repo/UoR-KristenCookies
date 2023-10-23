using JetBrains.Annotations;
using Logic.Infrastructure;

namespace Logic.Managers;

public interface ICustomerManager<TCustomer>
{
    [ UsedImplicitly ]
    Task<Result<TCustomer>> AddAsync( TCustomer customer );

    [ UsedImplicitly ]
    Task<Result<IEnumerable<TCustomer>>> GetAllAsync();

    [ UsedImplicitly ]
    Task<Result<TCustomer>> FindByEmailAsync( string email );
}