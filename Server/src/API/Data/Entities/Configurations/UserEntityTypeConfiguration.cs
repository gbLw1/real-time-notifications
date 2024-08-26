using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RTN.API.Data.Entities.Configurations;

public class UserEntityTypeConfiguration : BaseEntityTypeConfiguration<UserEntity>
{
    public override void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        base.Configure(builder);

        builder.Property(u => u.Name)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasMaxLength(250)
            .IsRequired();

        builder.HasMany(u => u.Logins)
        .WithOne(l => l.User)
        .HasForeignKey(l => l.UserId)
        .IsRequired();
    }
}
