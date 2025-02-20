using BaseRepository.tests.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace BaseRepository.tests.ProjectMocked.Infrastructure;
public partial class ProjectContext
{
    public DbSet<User> Users { get; set; }

    private static void ConfigureUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e => 
        {
            e.ToTable("Users");
            e.HasKey(e => e.Id);
            e.Property(c => c.EntityStatus);

            e.Property(c => c.Name);
            e.Property(c => c.Email);
            
            e.HasMany(e => e.BookLinks)
             .WithOne(e => e.User)
             .HasForeignKey(e => e.UserId);
        });
    }
}
