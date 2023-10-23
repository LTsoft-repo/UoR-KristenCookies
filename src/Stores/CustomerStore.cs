using DataModel.Infrastructure;
using DataModel.Model.Entities;
using JetBrains.Annotations;
using Stores.Infrastructure;

namespace Stores;

[ UsedImplicitly ]
public class CustomerStore : Store<ApplicationDbContext, Customer>
{
    public CustomerStore( ApplicationDbContext dbContext ) : base( dbContext ) { }

    public override IQueryable<Customer> GetAll()
    {
        return DatabaseContext.Customers
            .OrderBy( e => e.Name );
    }
}