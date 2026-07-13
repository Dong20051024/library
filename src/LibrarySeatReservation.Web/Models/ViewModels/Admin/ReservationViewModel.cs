using LibrarySeatReservation.Web.Models.Enums;

namespace LibrarySeatReservation.Web.Models.ViewModels.Admin;

public class ReservationViewModel
{
    public List<AdminReservationItem> Items { get; set; } = new();
    public ReservationStatus? CurrentStatus { get; set; }
    public string? ErrorMessage { get; set; }
}

public class AdminReservationItem
{
    public int Id { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public string StatusCssClass { get; set; } = string.Empty;
    public bool CanCheckIn { get; set; }
    public bool CanRelease { get; set; }
}
