using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DataModel.Model.Entities;

public record Order : Entity
{
    [ Required ]
    [ UsedImplicitly ]
    public Guid CustomerId { get; set; }

    [ UsedImplicitly ]
    public Customer Customer { get; set; } = null!;

    [ UsedImplicitly ]
    public List<OrderItem> Cookies { get; set; } = new();

    [ Required ]
    [ UsedImplicitly ]
    public DateTime OrderedAtUtc { get; set; }
}

public static class OrderExtensions
{
    [ UsedImplicitly ]
    public static void SetupOrderRelations( this ModelBuilder builder )
    {
        builder.Entity<Order>()
            .HasOne( x => x.Customer )
            .WithMany()
            .HasForeignKey( x => x.CustomerId )
            .IsRequired()
            .OnDelete( DeleteBehavior.Restrict );
    }

    [ UsedImplicitly ]
    public static void SetupOrderUniqueConstraints( this ModelBuilder builder ) { }
}