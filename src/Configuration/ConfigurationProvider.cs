using Autofac;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

namespace Configuration;

public interface IConfigurationProvider
{
    T Get<T>();
}

[ UsedImplicitly ]
public class ConfigurationProvider : IConfigurationProvider
{
    private readonly IContainer diContainer;

    public ConfigurationProvider( IConfiguration configuration, Action<ContainerBuilder> registerConfiguration )
    {
        var containerBuilder = new ContainerBuilder();
        containerBuilder.RegisterInstance( configuration ).SingleInstance();
        registerConfiguration( containerBuilder );
        diContainer = containerBuilder.Build();
    }

    public T Get<T>() => diContainer.Resolve<T>();
}