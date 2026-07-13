using LibrarySeatReservation.Web.Models.Entities;
using LibrarySeatReservation.Web.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibrarySeatReservation.Web.Data.Configurations;

public class SeatConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.ToTable("Seat");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).UseIdentityColumn();

        builder.Property(s => s.Floor).HasMaxLength(20).IsRequired();
        builder.Property(s => s.Area).HasMaxLength(50).IsRequired();
        builder.Property(s => s.SeatNumber).HasMaxLength(20).IsRequired();
        builder.Property(s => s.Status).HasConversion<int>().HasDefaultValue(SeatStatus.空闲);
        builder.Property(s => s.Facilities).HasMaxLength(200).HasDefaultValue("");
        builder.Property(s => s.IsActive).HasDefaultValue(true);

        builder.HasIndex(s => new { s.Floor, s.Area, s.SeatNumber })
            .IsUnique()
            .HasDatabaseName("UQ_Seat_Floor_Area_SeatNumber");
    }
}
