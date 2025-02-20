using BaseRepository.tests.Domain.Libraries;
using Microsoft.EntityFrameworkCore;

namespace BaseRepository.tests.ProjectMocked.Infrastructure;
public partial class ProjectContext
{
    public DbSet<Book> Books { get; set; }
    public DbSet<BookLink> BookLinks { get; set;}

    private static void ConfigureLibraries(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book>(e => 
        {
            e.ToTable("[Library].Books");
            e.HasKey(e => e.Id);
            e.Property(e => e.EntityStatus);

            e.Property(e => e.Name);
            e.Property(e => e.Author);
            e.Property(e => e.YearOfPublication);

            e.Property(e => e.Disponible);

            e.HasMany(e => e.BookLinks)
             .WithOne(e => e.Book)
             .HasForeignKey(e => e.BookId);
        });

        modelBuilder.Entity<BookLink>(e => 
        {
            e.ToTable("[Library].BookLinks");
            e.HasKey(b => new { b.UserId, b.BookId });
            e.Property(e => e.Id);
            e.Property(e => e.EntityStatus);
            
            e.Property(e => e.ExpiredDate);

            e.HasOne(e => e.User)
             .WithMany()
             .HasForeignKey(e => e.UserId);

            e.HasOne(e => e.Book)
             .WithMany(e => e.BookLinks)
             .HasForeignKey(e => e.BookId);
        });
    }
}