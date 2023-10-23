using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace Logging;

public static class LogConfigurationDefaults
{
    public const string Path = ".\\Logs";
    public const int MaxFileSizeInMegabytes = 10;
    public const int DebugLogRetainedFileCount = 5;
    public const int ErrorLogRetainedFileCount = 96;
}

public record LogConfiguration
{
    public string Path { get; init; } = LogConfigurationDefaults.Path;
    public int MaxFileSizeInMegabytes { get; init; } = LogConfigurationDefaults.MaxFileSizeInMegabytes;
    public int DebugLogRetainedFileCount { get; init; } = LogConfigurationDefaults.DebugLogRetainedFileCount;
    public int ErrorLogRetainedFileCount { get; init; } = LogConfigurationDefaults.ErrorLogRetainedFileCount;
}

// LogConfigurator is not tested.
public static class LogConfigurator
{
    public static void Configure( LogConfiguration configuration )
        => Configure( configuration, null );

    // ReSharper disable once MemberCanBePrivate.Global
    public static void Configure( LogConfiguration configuration, Action<LoggerConfiguration>? additionalConfiguration )
    {
        var loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override( "Microsoft", LogEventLevel.Information )
            .Enrich.FromLogContext()
            .WriteTo.Console( outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} " +
                                              "{Properties:j}{NewLine}{Exception}" )
            .WriteTo.Debug()
            .WriteTo.File( Path.Combine( configuration.Path, "debug-.log" ),
                fileSizeLimitBytes: 1024 * 1024 * configuration.MaxFileSizeInMegabytes,
                rollingInterval: RollingInterval.Hour,
                rollOnFileSizeLimit: true,
                retainedFileCountLimit: configuration.DebugLogRetainedFileCount,
                buffered: true,
                flushToDiskInterval: TimeSpan.FromSeconds( 5 ) )
            .WriteTo.File( Path.Combine( configuration.Path, "error-.log" ),
                fileSizeLimitBytes: 1024 * 1024 * configuration.MaxFileSizeInMegabytes,
                retainedFileCountLimit: configuration.ErrorLogRetainedFileCount,
                rollingInterval: RollingInterval.Hour,
                rollOnFileSizeLimit: true,
                buffered: true,
                flushToDiskInterval: TimeSpan.FromSeconds( 5 ),
                restrictedToMinimumLevel: LogEventLevel.Error );

        additionalConfiguration?.Invoke( loggerConfiguration );

        Log.Logger = loggerConfiguration.CreateLogger();
    }
}

// ReSharper disable once UnusedType.Global
public static class LogConfigurationLoader
{
    [ UsedImplicitly ]
    public static LogConfiguration LoadLogConfiguration( this IConfiguration configuration ) => new()
    {
        Path = configuration.GetValue( "Path", LogConfigurationDefaults.Path ),
        MaxFileSizeInMegabytes = configuration.GetValue( "MaxFileSizeInMegabytes", LogConfigurationDefaults.MaxFileSizeInMegabytes ),
        DebugLogRetainedFileCount = configuration.GetValue( "DebugLogRetainedFileCount", LogConfigurationDefaults.DebugLogRetainedFileCount ),
        ErrorLogRetainedFileCount = configuration.GetValue( "ErrorLogRetainedFileCount", LogConfigurationDefaults.ErrorLogRetainedFileCount )
    };
}

[ UsedImplicitly ]
public static class LogConfigurationExtensions
{
    [ UsedImplicitly ]
    public static Dictionary<string, string> GetConfigurationDictionary( this LogConfiguration configuration, string section )
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if( configuration == null )
        {
            return new();
        }

        return new()
        {
            [$"{section}:Path"] = configuration.Path,
            [$"{section}:MaxFileSizeInMegabytes"] = configuration.MaxFileSizeInMegabytes.ToString(),
            [$"{section}:DebugLogRetainedFileCount"] = configuration.DebugLogRetainedFileCount.ToString(),
            [$"{section}:ErrorLogRetainedFileCount"] = configuration.ErrorLogRetainedFileCount.ToString()
        };
    }
}