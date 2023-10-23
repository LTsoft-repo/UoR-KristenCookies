using Configuration.Extensions;
using ConfigurationProvider = Configuration.ConfigurationProvider;

namespace KristenCookiesMvc.Configuration;

public static class ConfigurationLoaderExtension
{
    public static ConfigurationProvider LoadConfiguration( this IConfigurationBuilder builder )
    {
        builder.AddDefaultConfiguration<Program>();

        var configuration = builder.Build();

        var configurationProvider = new ConfigurationProvider( configuration, b => b.RegisterConfiguration() );

        return configurationProvider;
    }
}