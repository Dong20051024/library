using LibrarySeatReservation.Web.Models.Enums;

namespace LibrarySeatReservation.Web.Models.Entities;

public class Reservation
{
    public int Id { get; set; }
    public int SeatId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ReservationStatus Status { get; set; } = ReservationStatus.待签到;
    public DateTime CreatedAt { get; set; }

    public Seat? Seat { get; set; }
}
