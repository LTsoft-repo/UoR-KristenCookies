namespace KristenCookiesMvc.Configuration;

public class AuthenticationConfiguration
{
    public string ClientId { get; init; } = string.Empty;
    public string ClientSecret { get; init; } = string.Empty;
}

public static class AuthenticationConfigurationLoader
{
    public static AuthenticationConfiguration LoadAuthenticationConfiguration( this IConfiguration configuration ) => new()
    {
        ClientId = configuration.GetValue<string>( "ClientId" ),
        ClientSecret = configuration.GetValue<string>( "ClientSecret" )
    };
}