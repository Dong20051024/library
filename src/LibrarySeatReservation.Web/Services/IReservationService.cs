using LibrarySeatReservation.Web.Models.Enums;

namespace LibrarySeatReservation.Web.Services;

public interface IReservationService
{
    void CreateReservation(int seatId, string studentName, DateTime startTime, DateTime endTime);
    void CancelReservation(int reservationId, string studentName);
    MyReservationsResult GetMyReservations(string studentName);
    AdminReservationResult GetAllReservations(ReservationStatus? statusFilter);
    void CheckIn(int reservationId);
    void Release(int reservationId);
}

public class MyReservationsResult
{
    public List<MyReservationItem> Items { get; set; } = new();
}

public class MyReservationItem
{
    public int Id { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ReservationStatus Status { get; set; }
}

public class AdminReservationResult
{
    public List<AdminReservationItemDto> Items { get; set; } = new();
    public ReservationStatus? CurrentStatus { get; set; }
}

public class AdminReservationItemDto
{
    public int Id { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ReservationStatus Status { get; set; }
}
