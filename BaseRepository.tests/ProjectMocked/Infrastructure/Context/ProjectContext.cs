using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BaseRepository.tests.ProjectMocked.Infrastructure;

public partial class ProjectContext(DbContextOptions<ProjectContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureUsers(modelBuilder);
        ConfigureLibraries(modelBuilder);
    }
}
