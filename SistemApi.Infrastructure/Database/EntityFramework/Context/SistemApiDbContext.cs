using Microsoft.EntityFrameworkCore;
using SistemApi.Infrastructure.Database.EntityFramework.Configurations.Auth;
using SistemApi.Infrastructure.Database.EntityFramework.Configurations.User;
using SistemApi.Infrastructure.Database.EntityFramework.Entities.Auth;
using SistemApi.Infrastructure.Database.EntityFramework.Entities.User;

namespace SistemApi.Infrastructure.Database.EntityFramework.Context;

public class SistemApiDbContext : DbContext
{
    public SistemApiDbContext(DbContextOptions<SistemApiDbContext> options) : base(options)
    {
    }

    public DbSet<UserEntity> User => Set<UserEntity>();
    public DbSet<RefreshTokenEntity> RefreshToken => Set<RefreshTokenEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}