using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Configuration.Extensions;

public static class ConfigurationBuilderExtensions
{
    /// <summary>
    ///     Default Configuration order:<br />
    ///     - Secrets<br />
    ///     - AddKeyPerFile (/mnt/secret-store")<br />
    ///     - Environment variables<br />
    ///     - Json file (appsettings.json)
    ///     <para>
    ///         <typeparamref name="T" /> is the class that will be used to define which Assembly the settings will load from.
    ///     </para>
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IConfigurationBuilder AddDefaultConfiguration<T>( this IConfigurationBuilder builder )
        where T : class
        =>
            builder
                .AddUserSecrets<T>( true, false )
                .ParseEmptyString( b => b.AddKeyPerFile( source =>
                {
                    const string directoryPath = "/mnt/secret-store";

                    if( Directory.Exists( directoryPath ) )
                    {
                        source.FileProvider =
                            new PhysicalFileProvider( directoryPath );
                    }

                    source.Optional = true;
                    source.ReloadOnChange = false;
                    source.SectionDelimiter = "--";
                } ) )
                .AddEnvironmentVariables()
                .AddJsonFile( "appsettings.json", true, false );
}