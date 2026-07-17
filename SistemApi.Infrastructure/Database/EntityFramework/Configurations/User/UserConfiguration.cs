using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemApi.Infrastructure.Database.EntityFramework.Entities.User;

namespace SistemApi.Infrastructure.Database.EntityFramework.Configurations.User;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("User");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email).IsRequired().HasMaxLength(200);
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.Password).IsRequired();
        builder.Property(u => u.Name).IsRequired().HasMaxLength(200);
        builder.Property(u => u.Role).IsRequired();


    }
}
