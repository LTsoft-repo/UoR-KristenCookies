using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DataModel.Model.Entities;

public record Order : Entity
{
    [ Required ]
    [ MaxLength( 500 ) ]
    public string CustomerName { get; set; } = null!;

    public List<OrderItem> Cookies { get; set; } = new();

    [ Required ]
    public DateTime OrderedAtUtc { get; set; }
}

public static class OrderExtensions
{
    public static void SetupOrderRelations( this ModelBuilder builder ) { }

    public static void SetupOrderUniqueConstraints( this ModelBuilder builder ) { }
}