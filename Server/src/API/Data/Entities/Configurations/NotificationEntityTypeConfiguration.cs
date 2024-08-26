using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RTN.API.Data.Entities.Configurations;

public class NotificationEntityConfiguration
    : BaseEntityTypeConfiguration<NotificationEntity>
{
    public override void Configure(EntityTypeBuilder<NotificationEntity> builder)
    {
        base.Configure(builder);

        builder.Property(n => n.Content)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(n => n.RedirectUrl)
            .HasMaxLength(250);

        builder.Property(n => n.IsRead)
            .IsRequired();

        builder.HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId)
            .IsRequired(false);
    }
}