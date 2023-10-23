using DataModel.Infrastructure;
using DataModel.Model;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Stores.Infrastructure;

public abstract class Store<TDbContext, TEntity> : IStore<TEntity>, IDisposable
    where TEntity : Entity
    where TDbContext : ApplicationDbContext
{
    private bool disposed;

    protected TDbContext DatabaseContext { get; }

    protected Store( TDbContext dbContext )
    {
        DatabaseContext = dbContext;
    }

    public virtual void Dispose()
    {
        disposed = true;
        GC.SuppressFinalize( this );
    }

    public virtual async Task<StoreResult<Guid>> AddAsync( TEntity entity, CancellationToken cancellationToken = default )
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        try
        {
            await DatabaseContext.AddAsync( entity, cancellationToken );
            await SaveChangesAsync( cancellationToken );

            return new()
            {
                Success = true,
                Data = entity.Id
            };
        }
        catch( Exception ex )
        {
            return new()
            {
                Success = false,
                Code = StoreErrorCode.AddException,
                Error = ex.Message,
                Exception = ex
            };
        }
    }

    public virtual async Task<StoreResult<TEntity>> GetAsync( Guid id, CancellationToken cancellationToken = default )
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        try
        {
            var entity = await GetByIdAsync( id );

            if( entity == null )
            {
                return new()
                {
                    Success = false,
                    Code = StoreErrorCode.NotFound,
                    Error = $"Entity {typeof( TEntity ).Name} with id {id} not found"
                };
            }

            return new()
            {
                Success = true,
                Data = entity
            };
        }
        catch( Exception ex )
        {
            return new()
            {
                Success = false,
                Code = StoreErrorCode.GetException,
                Error = ex.Message,
                Exception = ex
            };
        }
    }

    public virtual async Task<StoreResult<bool>> RemoveAsync( Guid id, CancellationToken cancellationToken = default )
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        try
        {
            var entity = await GetByIdAsync( id );

            if( entity == null )
            {
                return new()
                {
                    Success = false,
                    Code = StoreErrorCode.NotFound,
                    Error = $"Entity {typeof( TEntity ).Name} with id {id} not found"
                };
            }

            DatabaseContext.Remove( entity );
            await SaveChangesAsync( cancellationToken );

            return new()
            {
                Success = true,
                Data = true
            };
        }
        catch( Exception ex )
        {
            return new()
            {
                Success = false,
                Code = StoreErrorCode.RemoveException,
                Error = ex.Message,
                Exception = ex
            };
        }
    }

    public virtual async Task<StoreResult<TEntity>> UpdateAsync( TEntity entity, CancellationToken cancellationToken = default )
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        try
        {
            DatabaseContext.Update( entity );
            await SaveChangesAsync( cancellationToken );

            return new()
            {
                Success = true,
                Data = entity
            };
        }
        catch( Exception ex )
        {
            return new()
            {
                Success = false,
                Code = StoreErrorCode.UpdateException,
                Error = ex.Message,
                Exception = ex
            };
        }
    }

    public abstract IQueryable<TEntity> GetAll();

    public virtual async Task<StoreResult<IEnumerable<TEntity>>> GetAllAsync( CancellationToken cancellationToken = default )
    {
        cancellationToken.ThrowIfCancellationRequested();
        ThrowIfDisposed();

        try
        {
            var entities = await GetAll().ToListAsync( cancellationToken );

            return new()
            {
                Success = true,
                Data = entities
            };
        }
        catch( Exception ex )
        {
            return new()
            {
                Success = false,
                Code = StoreErrorCode.GetException,
                Error = ex.Message,
                Exception = ex
            };
        }
    }

    [ UsedImplicitly ]
    protected virtual void ThrowIfDisposed()
    {
        if( disposed )
        {
            throw new ObjectDisposedException( GetType().Name );
        }
    }

    [ UsedImplicitly ]
    protected virtual async Task SaveChangesAsync( CancellationToken cancellationToken = default ) =>
        await DatabaseContext.SaveChangesAsync( cancellationToken );

    [ UsedImplicitly ]
    protected virtual async Task<TEntity?> GetByIdAsync( Guid id )
    {
        var entity = await GetAll().FirstOrDefaultAsync( e => e.Id == id );

        return entity;
    }
}