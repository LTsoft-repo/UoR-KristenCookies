using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DataModel.Model.Entities;

[ UsedImplicitly ]
public record Customer : Entity

{
    [ Required ]
    [ MaxLength( 500 ) ]
    [ UsedImplicitly ]
    public string Name { get; set; } = "";

    [ Required ]
    [ MaxLength( 500 ) ]
    [ UsedImplicitly ]
    public string Email { get; set; } = null!;
}

public static class CustomerExtensions
{
    [ UsedImplicitly ]
    public static void SetupCustomerRelations( this ModelBuilder builder ) { }

    [ UsedImplicitly ]
    public static void SetupCustomerUniqueConstraints( this ModelBuilder builder )
    {
        builder.Entity<Customer>()
            .HasIndex( x => x.Email )
            .IsUnique();
    }
}