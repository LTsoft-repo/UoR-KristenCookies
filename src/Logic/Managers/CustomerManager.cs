using DataModel.Model.Entities;
using JetBrains.Annotations;
using Logic.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Stores.Infrastructure;

namespace Logic.Managers;

[ UsedImplicitly ]
public class CustomerManager : ICustomerManager<Customer>
{
    [ UsedImplicitly ]
    protected readonly IStore<Customer> CustomerStore;

    public CustomerManager( IStore<Customer> customerStore )
    {
        CustomerStore = customerStore;
    }

    public virtual async Task<Result<Customer>> AddAsync( Customer customer )
    {
        var canAddError = await CanAddAsync( customer );

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
            var addResult = await CustomerStore.AddAsync( customer );

            if( !addResult.Success )
            {
                return new()
                {
                    Success = false,
                    Code = ResultErrorCode.StoreError,
                    Error = addResult.Error
                };
            }

            var getResult = await CustomerStore.GetAsync( addResult.Data );

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

    public virtual async Task<Result<Customer>> FindByEmailAsync( string email )
    {
        var existingEntity = await CustomerStore.GetAll().FirstOrDefaultAsync( c => c.Email.ToLower() == email.ToLower() );

        if( existingEntity == null )
        {
            return new()
            {
                Success = false,
                Code = ResultErrorCode.DoesNotExist,
                Error = "The customer doesn't exists."
            };
        }

        return new()
        {
            Success = true,
            Data = existingEntity
        };
    }

    public virtual async Task<Result<IEnumerable<Customer>>> GetAllAsync()
    {
        try
        {
            var cookiesResult = await CustomerStore.GetAllAsync();

            if( !cookiesResult.Success )
            {
                return new()
                {
                    Success = false,
                    Code = ResultErrorCode.StoreError,
                    Error = cookiesResult.Error
                };
            }

            var customers = cookiesResult.Data;

            return new()
            {
                Success = true,
                Data = customers ?? Enumerable.Empty<Customer>()
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

    protected virtual string ValidateEntity( Customer entity )
    {
        if( string.IsNullOrWhiteSpace( entity.Name ) )
        {
            return "You must provide a name for the customer.";
        }

        if( string.IsNullOrWhiteSpace( entity.Email ) )
        {
            return "You must provide an e-mail for the customer.";
        }

        return "";
    }

    protected virtual async Task<string> CanAddAsync( Customer entity )
    {
        var isValidInput = ValidateEntity( entity );

        if( isValidInput != "" )
        {
            return isValidInput;
        }

        var existingEntity = await CustomerStore.GetAll().FirstOrDefaultAsync( c => c.Email.ToLower() == entity.Email.ToLower() );

        if( existingEntity != null )
        {
            return "The is already a customer with that email.";
        }

        return "";
    }
}