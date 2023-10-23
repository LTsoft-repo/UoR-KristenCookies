using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace KristenCookiesMvc.Infrastructure;

public interface IMigrationApplier
{
    [ UsedImplicitly ]
    void Apply();
}

[ UsedImplicitly ]
public class MigrationApplier<TDbContext> : IMigrationApplier
    where TDbContext : DbContext
{
    private readonly Func<TDbContext> dbContextFactory;

    public MigrationApplier( Func<TDbContext> dbContextFactory )
    {
        this.dbContextFactory = dbContextFactory;
    }

    public void Apply()
    {
        var dbContext = dbContextFactory();

        dbContext.Database.Migrate();
    }
}