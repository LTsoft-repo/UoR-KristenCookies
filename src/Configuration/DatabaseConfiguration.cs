using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

namespace Configuration;

public record DatabaseConfiguration
{
    public string ConnectionString { get; init; } = string.Empty;
}

[ UsedImplicitly ]
public static class DatabaseConfigurationLoader
{
    public static DatabaseConfiguration LoadDatabaseConfiguration( this IConfiguration configuration ) =>
        new()
        {
            ConnectionString = configuration.GetValue<string>( "DefaultConnection" ) ?? string.Empty
        };
}