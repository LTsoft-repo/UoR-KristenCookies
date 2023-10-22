using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Model;

public record Entity
{
    [ Key ]
    [ DatabaseGenerated( DatabaseGeneratedOption.Identity ) ]
    public Guid Id { get; init; }
}