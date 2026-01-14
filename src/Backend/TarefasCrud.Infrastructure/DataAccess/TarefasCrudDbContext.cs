using Microsoft.EntityFrameworkCore;
using TarefasCrud.Domain.Entities;

namespace TarefasCrud.Infrastructure.DataAccess;

public class TarefasCrudDbContext : DbContext
{
    public TarefasCrudDbContext(DbContextOptions options) : base(options) {}
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<TaskEntity> Tasks { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TarefasCrudDbContext).Assembly);
    }
    
}