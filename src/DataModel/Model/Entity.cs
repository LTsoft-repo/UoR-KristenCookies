using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;

namespace DataModel.Model;

public record Entity
{
    [ Key ]
    [ DatabaseGenerated( DatabaseGeneratedOption.Identity ) ]
    [ UsedImplicitly ]
    public Guid Id { get; init; }
}