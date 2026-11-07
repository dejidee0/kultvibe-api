using KultVibe.Domain.Entities;
using KultVibe.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KultVibe.Infrastructure.Persistence.Configurations;

public class WaitlistEntryConfiguration : IEntityTypeConfiguration<WaitlistEntry>
{
    public void Configure(EntityTypeBuilder<WaitlistEntry> builder)
    {
        builder.ToTable("WaitlistEntries");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.FullName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(w => w.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.HasIndex(w => w.Email)
            .IsUnique();

        builder.Property(w => w.GamerTag)
            .HasMaxLength(50);

        builder.Property(w => w.PrimaryVertical)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired(false);

        builder.Property(w => w.UserType)
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(UserType.Gamer)
            .IsRequired();

        builder.Property(w => w.PrimaryGame)
            .HasMaxLength(100);

        builder.Property(w => w.CompanyName)
            .HasMaxLength(150);

        builder.Property(w => w.Interests)
            .HasMaxLength(200);

        builder.Property(w => w.ReferralCode)
            .HasMaxLength(20);

        builder.Property(w => w.CreatedAt)
            .IsRequired();

        builder.Property(w => w.UpdatedAt)
            .IsRequired();
    }
}
