using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RTN.API.Data.Entities.Configurations;

public class LoginEntityTypeConfiguration : BaseEntityTypeConfiguration<LoginEntity> {
    public override void Configure(EntityTypeBuilder<LoginEntity> builder) {
        base.Configure(builder);

        builder.Property(l => l.PasswordHash)
            .IsRequired();

        builder.Property(l => l.RefreshToken)
            .HasMaxLength(250);

        builder.Property(l => l.RefreshTokenExpirationTime);

        builder.HasOne(l => l.User)
            .WithMany(u => u.Logins)
            .HasForeignKey(l => l.UserId)
            .IsRequired();
    }
}
