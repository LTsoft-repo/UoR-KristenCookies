using DataModel.Infrastructure;
using DataModel.Model.Entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Stores.Infrastructure;

namespace Stores;

[ UsedImplicitly ]
public class OrderStore : Store<ApplicationDbContext, Order>
{
    public OrderStore( ApplicationDbContext dbContext ) : base( dbContext ) { }

    public override IQueryable<Order> GetAll()
    {
        return DatabaseContext.Orders
            .Include( e => e.Customer )
            .Include( e => e.Cookies )
            .ThenInclude( c => c.Cookie )
            .OrderBy( e => e.OrderedAtUtc );
    }
}