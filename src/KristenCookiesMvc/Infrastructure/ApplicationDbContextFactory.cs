// See https://aka.ms/new-console-template for more information

using Configuration;
using DataModel.Infrastructure;
using JetBrains.Annotations;
using KristenCookiesMvc.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EmailConfirmationsFetcher.DataModel;

[ UsedImplicitly ]
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext( string[] args )
    {
        var configurationProvider = new ConfigurationBuilder().LoadConfiguration();
        var dbConfiguration = configurationProvider.Get<DatabaseConfiguration>();

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer( dbConfiguration.ConnectionString );

        return new( optionsBuilder.Options );
    }
}