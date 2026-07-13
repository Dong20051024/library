using LibrarySeatReservation.Web.Models.Enums;

namespace LibrarySeatReservation.Web.Services;

public interface ISeatService
{
    SeatListResult GetSeatList(string? floor);
    SeatDetailResult? GetSeatDetail(int id);
}

public class SeatListResult
{
    public List<SeatListItem> Seats { get; set; } = new();
    public string? CurrentFloor { get; set; }
    public List<string> Floors { get; set; } = new();
}

public class SeatListItem
{
    public int Id { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public SeatStatus Status { get; set; }
    public string Facilities { get; set; } = string.Empty;
}

public class SeatDetailResult
{
    public int Id { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public SeatStatus Status { get; set; }
    public string Facilities { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
