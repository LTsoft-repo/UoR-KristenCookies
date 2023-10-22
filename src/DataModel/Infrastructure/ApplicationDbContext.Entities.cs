using DataModel.Model.Entities;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DataModel.Infrastructure;

public partial class ApplicationDbContext
{
    [ UsedImplicitly ]
    public DbSet<Cookie> Cookies { get; set; } = null!;

    [ UsedImplicitly ]
    public DbSet<Order> Orders { get; set; } = null!;

    [ UsedImplicitly ]
    public DbSet<OrderItem> OrderItems { get; set; } = null!;

    protected virtual void AddEntities( ModelBuilder builder )
    {
        builder.SetupCookieRelations();
        builder.SetupCookieUniqueConstraints();

        builder.SetupOrderRelations();
        builder.SetupOrderUniqueConstraints();

        builder.SetupOrderItemRelations();
        builder.SetupOrderItemUniqueConstraints();
    }
}