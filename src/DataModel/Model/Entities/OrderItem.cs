using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DataModel.Model.Entities;

public record OrderItem : Entity
{
    [ Required ]
    public Guid OrderId { get; set; }

    public Order Order { get; set; } = null!;

    [ Required ]
    public Guid CookieId { get; set; }

    public Cookie Cookie { get; set; } = null!;

    [ Required ]
    public int Quantity { get; set; }
}

public static class OrderItemExtensions
{
    public static void SetupOrderItemRelations( this ModelBuilder builder )
    {
        builder.Entity<OrderItem>()
            .HasOne( x => x.Order )
            .WithMany( x => x.Cookies )
            .HasForeignKey( x => x.OrderId )
            .IsRequired()
            .OnDelete( DeleteBehavior.Cascade );

        builder.Entity<OrderItem>()
            .HasOne( x => x.Cookie )
            .WithMany()
            .HasForeignKey( x => x.CookieId )
            .IsRequired()
            .OnDelete( DeleteBehavior.Restrict );
    }

    public static void SetupOrderItemUniqueConstraints( this ModelBuilder builder ) { }
}