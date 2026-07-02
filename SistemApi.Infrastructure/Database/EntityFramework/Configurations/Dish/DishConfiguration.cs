using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemApi.Infrastructure.Database.EntityFramework.Entities.Dish;

namespace SistemApi.Infrastructure.Database.EntityFramework.Configurations.Dish;

public class DishConfiguration : IEntityTypeConfiguration<DishEntity>
{
    public void Configure(EntityTypeBuilder<DishEntity> builder)
    {
        builder.ToTable("Dish");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Name).IsRequired().HasMaxLength(150);
        builder.Property(d => d.Price).HasColumnType("numeric(10,2)");
    }
}
