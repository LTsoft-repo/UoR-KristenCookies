using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Configuration.Extensions;

public static class EmptyStringExtensions
{
    public static IConfigurationBuilder ParseEmptyString( this IConfigurationBuilder configurationBuilder, Action<IConfigurationBuilder> registerSource )
    {
        registerSource( new WrappedConfigurationBuilder( configurationBuilder ) );

        return configurationBuilder;
    }

    private class WrappedConfigurationBuilder : IConfigurationBuilder
    {
        private readonly IConfigurationBuilder baseBuilder;

        public WrappedConfigurationBuilder( IConfigurationBuilder baseBuilder )
        {
            this.baseBuilder = baseBuilder;
        }

        public IConfigurationBuilder Add( IConfigurationSource source ) => baseBuilder.Add( new WrappedConfigurationSource( source ) );

        public IConfigurationRoot Build() => baseBuilder.Build();

        public IDictionary<string, object> Properties => baseBuilder.Properties;
        public IList<IConfigurationSource> Sources => baseBuilder.Sources;
    }

    private class WrappedConfigurationSource : IConfigurationSource
    {
        private readonly IConfigurationSource baseSource;

        public WrappedConfigurationSource( IConfigurationSource baseSource )
        {
            this.baseSource = baseSource;
        }

        public Microsoft.Extensions.Configuration.IConfigurationProvider Build( IConfigurationBuilder builder )
            => new CleanEmptyStringConfigurationProvider( baseSource.Build( builder ) );
    }

    private class CleanEmptyStringConfigurationProvider : Microsoft.Extensions.Configuration.IConfigurationProvider
    {
        private readonly Microsoft.Extensions.Configuration.IConfigurationProvider baseProvider;

        public CleanEmptyStringConfigurationProvider( Microsoft.Extensions.Configuration.IConfigurationProvider baseProvider )
        {
            this.baseProvider = baseProvider;
        }

        public bool TryGet( string key, out string value )
        {
            if( !baseProvider.TryGet( key, out var newValue ) )
            {
                value = "<EmptyString>";

                return false;
            }

            value = newValue?.Replace( "<EmptyString>", "" ) ?? "";

            return true;
        }

        public void Set( string key, string? value ) => baseProvider.Set( key, value );

        public IChangeToken GetReloadToken() => baseProvider.GetReloadToken();

        public void Load() => baseProvider.Load();

        public IEnumerable<string> GetChildKeys( IEnumerable<string> earlierKeys, string? parentPath )
            => baseProvider.GetChildKeys( earlierKeys, parentPath );
    }
}