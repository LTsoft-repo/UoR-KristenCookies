using DataModel.Infrastructure;
using DataModel.Model.Entities;
using JetBrains.Annotations;
using Stores.Infrastructure;

namespace Stores;

[ UsedImplicitly ]
public class CookieStore : Store<ApplicationDbContext, Cookie>
{
    public CookieStore( ApplicationDbContext dbContext ) : base( dbContext ) { }

    public override IQueryable<Cookie> GetAll()
    {
        return DatabaseContext.Cookies
            .OrderBy( e => e.Name );
    }
}