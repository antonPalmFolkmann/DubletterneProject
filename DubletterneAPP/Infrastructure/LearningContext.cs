using Microsoft.EntityFrameworkCore;

namespace Infrastructure;
public class ComicsContext : DbContext, ILearningContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Resource> Resources => Set<Resource>();
    public ComicsContext(DbContextOptions options) : base(options){}
}