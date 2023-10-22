using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DataModel.Model.Entities;

public record Order : Entity
{
    [ Required ]
    [ MaxLength( 500 ) ]
    [ UsedImplicitly ]
    public string CustomerName { get; set; } = null!;

    [ UsedImplicitly ]
    public List<OrderItem> Cookies { get; set; } = new();

    [ Required ]
    [ UsedImplicitly ]
    public DateTime OrderedAtUtc { get; set; }
}

public static class OrderExtensions
{
    [ UsedImplicitly ]
    public static void SetupOrderRelations( this ModelBuilder builder ) { }

    [ UsedImplicitly ]
    public static void SetupOrderUniqueConstraints( this ModelBuilder builder ) { }
}