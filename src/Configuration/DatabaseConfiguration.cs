using Microsoft.Extensions.Configuration;

namespace Configuration;

public record DatabaseConfiguration
{
    public string ConnectionString { get; init; }
}

public static class DatabaseConfigurationLoader
{
    public static DatabaseConfiguration LoadDatabaseConfiguration( this IConfiguration configuration ) =>
        new()
        {
            ConnectionString = configuration.GetValue<string>( "DefaultConnection" )
        };
}