
namespace Infrastructure;
public class LearningContext : DbContext, ILearningContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Resource> Resources => Set<Resource>();
    public DbSet<TextParagraph> TextParagraphs => Set<TextParagraph>();
    public LearningContext(DbContextOptions options) : base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder){
        modelBuilder.Entity<User>()
                    .HasIndex(u => u.UserName)
                    .IsUnique();

        modelBuilder.Entity<Resource>()
                    .HasIndex(r => r.Title)
                    .IsUnique();

        modelBuilder.Entity<TextParagraph>()
                    .HasIndex(t => t.Id)
                    .IsUnique();
    }

}