using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace BaseRepository.ContextBase;
public class BaseDbContext(DbContextOptions<DbContext> options, 
                           Assembly entitiesAssembly) : DbContext(options)
{
    private readonly Assembly _databaseEntitiesAssembly = entitiesAssembly;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(_databaseEntitiesAssembly);
    }
}
