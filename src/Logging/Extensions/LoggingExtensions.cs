using Autofac;
using Autofac.Extensions.DependencyInjection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Logging.Extensions;

public static class LoggingExtensions
{
    [ UsedImplicitly ]
    public static ContainerBuilder AddSerilog( this ContainerBuilder builder, LogConfiguration configuration )
    {
        _ = configuration ?? throw new ArgumentNullException( nameof( configuration ) );

        var services = new ServiceCollection();
        services.AddLogging( b => b.AddSerilog( dispose: true ) );
        builder.Populate( services );

        LogConfigurator.Configure( configuration );

        return builder;
    }
}