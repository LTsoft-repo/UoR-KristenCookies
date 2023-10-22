using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DataModel.Model.Entities;

public record Cookie : Entity
{
    [ Required ]
    [ MaxLength( 200 ) ]
    public string Name { get; set; } = null!;
}

public static class CookieExtensions
{
    public static void SetupCookieRelations( this ModelBuilder builder ) { }

    public static void SetupCookieUniqueConstraints( this ModelBuilder builder )
    {
        builder.Entity<Cookie>()
            .HasIndex( x => x.Name )
            .IsUnique();
    }
}