using LibrarySeatReservation.Web.Data.Configurations;
using LibrarySeatReservation.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibrarySeatReservation.Web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Seat> Seats => Set<Seat>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<Admin> Admins => Set<Admin>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new SeatConfiguration());
        modelBuilder.ApplyConfiguration(new ReservationConfiguration());
        modelBuilder.ApplyConfiguration(new AdminConfiguration());
    }
}
