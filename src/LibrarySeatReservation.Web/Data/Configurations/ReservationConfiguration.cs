using LibrarySeatReservation.Web.Models.Entities;
using LibrarySeatReservation.Web.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibrarySeatReservation.Web.Data.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("Reservation");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).UseIdentityColumn();

        builder.Property(r => r.StudentName).HasMaxLength(50).IsRequired();
        builder.Property(r => r.StartTime).HasColumnType("datetime2").IsRequired();
        builder.Property(r => r.EndTime).HasColumnType("datetime2").IsRequired();
        builder.Property(r => r.Status).HasConversion<int>().HasDefaultValue(ReservationStatus.待签到);
        builder.Property(r => r.CreatedAt).HasColumnType("datetime2").HasDefaultValueSql("GETDATE()");

        builder.HasOne(r => r.Seat)
            .WithMany(s => s.Reservations)
            .HasForeignKey(r => r.SeatId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasIndex(r => r.SeatId).HasDatabaseName("IX_Reservation_SeatId");
        builder.HasIndex(r => r.StudentName).HasDatabaseName("IX_Reservation_StudentName");
        builder.HasIndex(r => r.Status).HasDatabaseName("IX_Reservation_Status");
        builder.HasIndex(r => new { r.SeatId, r.StartTime, r.EndTime })
            .HasDatabaseName("IX_Reservation_SeatId_TimeRange");
    }
}
