using Autofac;
using DataModel.Model.Entities;
using JetBrains.Annotations;
using Logic.Managers;
using Microsoft.EntityFrameworkCore;
using Stores;
using Stores.Infrastructure;

namespace KristenCookiesMvc.Infrastructure;

public static class ServiceExtensions
{
    [ UsedImplicitly ]
    public static IServiceCollection AddDbContext<TDbContext>(
        this IServiceCollection services,
        string dbConnectionString,
        ServiceLifetime dbContextLifetime )
        where TDbContext : DbContext
    {
        services.AddDbContext<TDbContext>( options => options.UseSqlServer( dbConnectionString ),
            dbContextLifetime );

        services.Add( new( typeof( DbContext ),
            s => s.GetService<TDbContext>() ?? throw new NullReferenceException( "No service registered for DbContext." ),
            dbContextLifetime ) );

        services.AddTransient<IMigrationApplier, MigrationApplier<TDbContext>>();

        return services;
    }

    public static ContainerBuilder RegisterServices( this ContainerBuilder builder )
    {
        builder.RegisterType<CookieStore>().As<IStore<Cookie>>();
        builder.RegisterType<OrderStore>().As<IStore<Order>>();
        builder.RegisterType<CustomerStore>().As<IStore<Customer>>();

        builder.RegisterType<CookieManager>().As<ICookieManager<Cookie>>();
        builder.RegisterType<OrderManager>().As<IOrderManager<Order>>();
        builder.RegisterType<CustomerManager>().As<ICustomerManager<Customer>>();

        return builder;
    }
}