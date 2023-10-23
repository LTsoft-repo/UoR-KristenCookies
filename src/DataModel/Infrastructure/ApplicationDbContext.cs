using Microsoft.EntityFrameworkCore;

namespace DataModel.Infrastructure;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext( DbContextOptions<ApplicationDbContext> options ) :
        base( options ) { }

    protected override void OnModelCreating( ModelBuilder builder )
    {
        base.OnModelCreating( builder );

        AddEntities( builder );
    }
}