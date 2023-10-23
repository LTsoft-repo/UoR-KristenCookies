using Autofac;
using DataModel.Infrastructure;
using JetBrains.Annotations;

namespace KristenCookiesMvc.Infrastructure;

public static class Migrations
{
    [ UsedImplicitly ]
    public static bool ApplyMigrations( IComponentContext context )
    {
        var migrationAppliers = context.Resolve<IEnumerable<IMigrationApplier>>().ToList();

        var logger = context.Resolve<ILogger>();

        return ApplyMigrations( migrationAppliers, logger );
    }

    [ UsedImplicitly ]
    public static bool ApplyMigrations( IServiceProvider services )
    {
        var migrationAppliers = services.GetRequiredService<IEnumerable<IMigrationApplier>>().ToList();

        var logger = services.GetRequiredService<ILogger>();

        return ApplyMigrations( migrationAppliers, logger );
    }

    private static bool ApplyMigrations( List<IMigrationApplier> migrationAppliers, ILogger logger )
    {
        var migrationComplete = true;

        migrationAppliers.ForEach(
            applier =>
            {
                try
                {
                    applier.Apply();
                }
                catch( Exception e )
                {
                    migrationComplete = false;
                    logger.LogError( e, "Database Migrations failed" );
                }
            } );

        return migrationComplete;
    }

    [ UsedImplicitly ]
    public static ContainerBuilder AddMigrationAppliers( this ContainerBuilder builder )
    {
        builder.RegisterType<MigrationApplier<ApplicationDbContext>>()
            .As<IMigrationApplier>();

        return builder;
    }
}