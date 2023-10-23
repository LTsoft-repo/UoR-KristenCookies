using Autofac;
using Configuration;
using JetBrains.Annotations;
using Logging;

namespace KristenCookiesMvc.Configuration;

// ReSharper disable once InconsistentNaming
public static class ConfigurationIOC
{
    [ UsedImplicitly ]
    public static ContainerBuilder RegisterConfiguration( this ContainerBuilder builder )
    {
        builder.Register( context => context.Resolve<IConfiguration>()
                .GetSection( ConfigurationSections.Log )
                .LoadLogConfiguration() )
            .SingleInstance();

        builder.Register( context => new DatabaseConfiguration
            {
                ConnectionString = context.Resolve<IConfiguration>()
                                       .GetConnectionString( "DefaultConnection" ) ??
                                   ""
            } )
            .As<DatabaseConfiguration>()
            .SingleInstance();

        builder.Register( context => context.Resolve<IConfiguration>()
                .GetSection( ConfigurationSections.Authentication )
                .LoadAuthenticationConfiguration() )
            .SingleInstance();

        return builder;
    }
}