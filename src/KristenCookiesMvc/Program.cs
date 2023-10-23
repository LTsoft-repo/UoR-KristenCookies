using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutofacExtensions;
using Configuration;
using Configuration.Extensions;
using DataModel.Infrastructure;
using JetBrains.Annotations;
using KristenCookiesMvc.Configuration;
using KristenCookiesMvc.Infrastructure;
using Logging;
using Microsoft.AspNetCore.Authentication.Cookies;
using Serilog;
using ConfigurationProvider = Configuration.ConfigurationProvider;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace KristenCookiesMvc;

public class Program
{
    public static void Main( string[] args )
    {
        var configurationProvider = new ConfigurationBuilder().LoadConfiguration();
        ConfigureLog( configurationProvider );

        try
        {
            #region Builder
            var builder = WebApplication.CreateBuilder( args );

            builder.Host.UseSerilog()
                .UseServiceProviderFactory( new AutofacServiceProviderFactory() )
                .ConfigureAppConfiguration( c => c.AddDefaultConfiguration() )
                .ConfigureContainer<ContainerBuilder>( ConfigureContainer );

            // Gets the configurations.
            var authenticationConfiguration = configurationProvider.Get<AuthenticationConfiguration>();
            var databaseConfiguration = configurationProvider.Get<DatabaseConfiguration>();

            // Add services to the container.
            builder.Services.AddLogging( b => b.AddSerilog( dispose: true ) );

            // Adds Google Authentication.
            builder.Services.AddAuthentication( options => { options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; } )
                .AddCookie( options =>
                {
                    options.LoginPath = "/account/google-login"; // Must be lowercase
                } )
                .AddGoogle( options =>
                {
                    options.ClientId = authenticationConfiguration.ClientId;
                    options.ClientSecret = authenticationConfiguration.ClientSecret;
                } );

            // Adds the database context.
            builder.Services.AddDbContext<ApplicationDbContext>(
                databaseConfiguration.ConnectionString,
                ServiceLifetime.Scoped );

            builder.Services.AddControllersWithViews();
            #endregion

            var app = builder.Build();

            #region App
            // Configure the HTTP request pipeline.
            if( !app.Environment.IsDevelopment() )
            {
                app.UseExceptionHandler( "/Home/Error" );
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                "default",
                "{controller=Home}/{action=Index}/{id?}" );

            var migrationsApplied = Migrations.ApplyMigrations( app.Services );

            if( !migrationsApplied )
            {
                throw new( "Migrations were not applied" );
            }
            #endregion

            app.Run();
        }
        catch( Exception ex )
        {
            if( ex.GetType().Name.Equals( "StopTheHostException", StringComparison.Ordinal ) )
            {
                throw;
            }

            Log.Fatal( ex, $"Host terminated unexpectedly ({ex.GetType().Name})" );
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static void ConfigureContainer( ContainerBuilder builder )
    {
        // Register the configuration.
        builder.RegisterConfiguration();

        // Registers the logger.
        builder.Register<ILogger>( c => c.Resolve<ILogger<Program>>() );
        builder.ReplaceTypeWithGenericTypeBasedOnRequestingType<ILogger, ILogger<object>>();

        // Registers the Managers and Stores.
        builder.RegisterServices();
    }

    public static void ConfigureLog( ConfigurationProvider configurationProvider )
    {
        //var configuration = GetLogConfiguration();
        var configuration = configurationProvider.Get<LogConfiguration>();
        LogConfigurator.Configure( configuration );
    }
}

public static class ProgramExtensions
{
    [ UsedImplicitly ]
    public static IConfigurationBuilder AddDefaultConfiguration( this IConfigurationBuilder configurationBuilder ) =>
        configurationBuilder.AddDefaultConfiguration<Program>();
}