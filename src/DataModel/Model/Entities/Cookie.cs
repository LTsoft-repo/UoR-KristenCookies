using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
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
    [ UsedImplicitly ]
    public static void SetupCookieRelations( this ModelBuilder builder ) { }

    [ UsedImplicitly ]
    public static void SetupCookieUniqueConstraints( this ModelBuilder builder )
    {
        builder.Entity<Cookie>()
            .HasIndex( x => x.Name )
            .IsUnique();
    }
}