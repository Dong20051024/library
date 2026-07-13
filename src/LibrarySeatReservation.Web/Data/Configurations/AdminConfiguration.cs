using LibrarySeatReservation.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibrarySeatReservation.Web.Data.Configurations;

public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.ToTable("Admin");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id).UseIdentityColumn();

        builder.Property(a => a.Username).HasMaxLength(50).IsRequired();
        builder.Property(a => a.Password).HasMaxLength(100).IsRequired();
        builder.Property(a => a.DisplayName).HasMaxLength(50).IsRequired();

        builder.HasIndex(a => a.Username)
            .IsUnique()
            .HasDatabaseName("UQ_Admin_Username");
    }
}
