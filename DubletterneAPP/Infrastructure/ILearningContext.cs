using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public interface ILearningContext : IDisposable
    {
        DbSet<User> Users { get; }
        DbSet<Resource> Resources { get; }
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}