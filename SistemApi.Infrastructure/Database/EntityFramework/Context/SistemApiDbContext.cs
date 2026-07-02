using Microsoft.EntityFrameworkCore;
using SistemApi.Infrastructure.Database.EntityFramework.Configurations.Dish;
using SistemApi.Infrastructure.Database.EntityFramework.Configurations.User;
using SistemApi.Infrastructure.Database.EntityFramework.Entities.Dish;
using SistemApi.Infrastructure.Database.EntityFramework.Entities.User;

namespace SistemApi.Infrastructure.Database.EntityFramework.Context;

public class SistemApiDbContext : DbContext
{
    public SistemApiDbContext(DbContextOptions<SistemApiDbContext> options) : base(options)
    {
    }

    public DbSet<DishEntity> Dish => Set<DishEntity>();
    public DbSet<UserEntity> User => Set<UserEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DishConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
