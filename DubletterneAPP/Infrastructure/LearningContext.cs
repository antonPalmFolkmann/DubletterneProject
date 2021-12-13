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

        var connectionString = "Server=tcp:dubletterne.database.windows.net,1433;Initial Catalog=Project;Persist Security Info=False;User ID={yourUser};Password={yourPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Authentication=\"Active Directory Password\";";

        var optionsBuilder = new DbContextOptionsBuilder<LearningContext>().UseSqlServer(connectionString);

        return new LearningContext(optionsBuilder.Options);
    }
}