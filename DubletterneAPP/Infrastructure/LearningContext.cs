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
                    .IsUnique();                   ;

        modelBuilder.Entity<Resource>()
                    .HasIndex(r => r.Title)
                    .IsUnique();

        modelBuilder.Entity<TextParagraph>()
                    .HasIndex(t => t.Id)
                    .IsUnique();
    }

}

public class LearningContextFactory : IDesignTimeDbContextFactory<LearningContext>
{
    public LearningContext CreateDbContext(string[] args)
    {

        var connectionString = "Server=dubletterne.database.windows.net; Authentication=Active Directory Service Principal; Database=Project; User Id=94eabc4d-57ab-4d90-b5d8-92dad5c49f2e; Password=x~-7Q~th26rtfoBxGJx3aZF0IZAWedtFuXhGM";

        var optionsBuilder = new DbContextOptionsBuilder<LearningContext>().UseSqlServer(connectionString);

        return new LearningContext(optionsBuilder.Options);
    }
}