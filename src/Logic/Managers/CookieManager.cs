using DataModel.Model.Entities;
using JetBrains.Annotations;
using Logic.Infrastructure;
using Stores.Infrastructure;

namespace Logic.Managers;

[ UsedImplicitly ]
public class CookieManager : ICookieManager<Cookie>
{
    [ UsedImplicitly ]
    protected readonly IStore<Cookie> CookieStore;

    public CookieManager( IStore<Cookie> cookieStore )
    {
        CookieStore = cookieStore;
    }

    public virtual async Task<Result<Cookie>> AddAsync( string name )
    {
        var cookie = new Cookie { Name = name };

        var canAddError = CanAdd( cookie );

        if( canAddError != "" )
        {
            return new()
            {
                Success = false,
                Error = canAddError
            };
        }

        try
        {
            var addResult = await CookieStore.AddAsync( cookie );

            if( !addResult.Success )
            {
                return new()
                {
                    Success = false,
                    Code = ResultErrorCode.StoreError,
                    Error = addResult.Error
                };
            }

            var getResult = await CookieStore.GetAsync( addResult.Data );

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

    public virtual async Task<Result<IEnumerable<Cookie>>> GetAllAsync()
    {
        try
        {
            var cookiesResult = await CookieStore.GetAllAsync();

            if( !cookiesResult.Success )
            {
                return new()
                {
                    Success = false,
                    Code = ResultErrorCode.StoreError,
                    Error = cookiesResult.Error
                };
            }

            var cookies = cookiesResult.Data;

            return new()
            {
                Success = true,
                Data = cookies ?? Enumerable.Empty<Cookie>()
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

    public virtual async Task<Result<Cookie>> GetAsync( Guid id )
    {
        try
        {
            var result = await CookieStore.GetAsync( id );

            if( !result.Success )
            {
                return new()
                {
                    Success = false,
                    Code = ResultErrorCode.StoreError,
                    Error = result.Error
                };
            }

            return new()
            {
                Success = true,
                Data = result.Data
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

    public virtual async Task<Result<Cookie>> RemoveAsync( Guid id )
    {
        try
        {
            var cookieResult = await GetAsync( id );

            if( !cookieResult.Success )
            {
                return cookieResult;
            }

            var cookie = cookieResult.Data;

            var result = await CookieStore.RemoveAsync( id );

            if( !result.Success )
            {
                return new()
                {
                    Success = false,
                    Code = ResultErrorCode.StoreError,
                    Error = result.Error
                };
            }

            return new()
            {
                Success = true,
                Data = cookie
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

    public virtual async Task<Result<Cookie>> UpdateAsync( Guid id, string name )
    {
        try
        {
            var cookieResult = await GetAsync( id );

            if( !cookieResult.Success )
            {
                return cookieResult;
            }

            var cookie = cookieResult.Data! with { Name = name };

            var isValidCookie = ValidateCookie( cookie );

            if( isValidCookie != "" )
            {
                return new()
                {
                    Success = false,
                    Code = ResultErrorCode.StoreError,
                    Error = isValidCookie
                };
            }

            var result = await CookieStore.UpdateAsync( cookie );

            if( !result.Success )
            {
                return new()
                {
                    Success = false,
                    Code = ResultErrorCode.StoreError,
                    Error = result.Error
                };
            }

            return new()
            {
                Success = true,
                Data = cookie
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

    protected virtual string ValidateCookie( Cookie cookie )
    {
        if( string.IsNullOrWhiteSpace( cookie.Name ) )
        {
            return "You must provide which cookie you are adding.";
        }

        return "";
    }

    protected virtual string CanAdd( Cookie cookie )
    {
        var isValidInput = ValidateCookie( cookie );

        if( isValidInput != "" )
        {
            return isValidInput;
        }

        var existingCookie = CookieStore.GetAll().FirstOrDefault( c => c.Name.ToLower() == cookie.Name.ToLower() );

        if( existingCookie != null )
        {
            return "The is already a cookie with that name.";
        }

        return "";
    }
}