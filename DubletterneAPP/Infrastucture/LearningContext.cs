using Microsoft.EntityFrameworkCore;
using Core;

namespace Infrastructure;
public class ComicsContext : DbContext, ILearningContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Resource> Resources => Set<Resource>();
    public ComicsContext(DbContextOptions options) : base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder){
        modelBuilder.Entity<User>()
                    .HasIndex(u => u.UserName)
                    .IsUnique();

        modelBuilder.Entity<Resource>()
                    .HasIndex(r => r.Title)
                    .IsUnique();
    }
}