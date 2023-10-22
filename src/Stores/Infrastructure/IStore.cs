namespace Stores.Infrastructure;

public interface IStore<T> where T : class
{
    Task<StoreResult<Guid>> AddAsync( T entity, CancellationToken cancellationToken = default );

    Task<StoreResult<T>> GetAsync( Guid id, CancellationToken cancellationToken = default );

    IQueryable<T> GetAll();

    Task<StoreResult<IEnumerable<T>>> GetAllAsync( CancellationToken cancellationToken = default );

    Task<StoreResult<bool>> RemoveAsync( Guid id, CancellationToken cancellationToken = default );

    Task<StoreResult<T>> UpdateAsync( T entity, CancellationToken cancellationToken = default );
}