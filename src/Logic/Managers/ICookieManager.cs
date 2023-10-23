using Logic.Infrastructure;

namespace Logic.Managers;

public interface ICookieManager<TCookie>
{
    Task<Result<TCookie>> AddAsync( string name );

    Task<Result<TCookie>> RemoveAsync( Guid id );

    Task<Result<TCookie>> UpdateAsync( Guid id, string name );

    Task<Result<IEnumerable<TCookie>>> GetAllAsync();

    Task<Result<TCookie>> GetAsync( Guid id );
}