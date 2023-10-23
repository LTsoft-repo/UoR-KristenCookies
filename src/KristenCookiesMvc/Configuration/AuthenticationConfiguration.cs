namespace KristenCookiesMvc.Configuration;

public class AuthenticationConfiguration
{
    public string ClientId { get; init; }
    public string ClientSecret { get; init; }
}

public static class AuthenticationConfigurationLoader
{
    public static AuthenticationConfiguration LoadAuthenticationConfiguration( this IConfiguration configuration ) => new()
    {
        ClientId = configuration.GetValue<string>( "ClientId" ),
        ClientSecret = configuration.GetValue<string>( "ClientSecret" )
    };
}