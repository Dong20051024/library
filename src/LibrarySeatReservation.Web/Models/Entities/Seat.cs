using LibrarySeatReservation.Web.Models.Enums;

namespace LibrarySeatReservation.Web.Models.Entities;

public class Seat
{
    public int Id { get; set; }
    public string Floor { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string SeatNumber { get; set; } = string.Empty;
    public SeatStatus Status { get; set; } = SeatStatus.空闲;
    public string Facilities { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;

    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
