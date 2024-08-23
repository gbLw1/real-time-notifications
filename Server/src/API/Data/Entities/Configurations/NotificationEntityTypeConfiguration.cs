using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RTN.API.Data.Entities.Configurations;

public class NotificationEntityConfiguration
    : BaseEntityTypeConfiguration<NotificationEntity>
{
    public override void Configure(EntityTypeBuilder<NotificationEntity> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.Content)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(p => p.RedirectUrl)
            .HasMaxLength(250);

        builder.Property(p => p.IsRead)
            .IsRequired();
    }
}