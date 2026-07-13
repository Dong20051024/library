using LibrarySeatReservation.Web.Models.Enums;

namespace LibrarySeatReservation.Web.Services;

public interface ISeatService
{
    SeatListResult GetSeatList(string? floor);
    SeatDetailResult? GetSeatDetail(int id);

    // ▼ 管理端座位 CRUD
    AdminSeatListResult GetAllSeats();
    void CreateSeat(string floor, string area, string seatNumber, string facilities);
    void UpdateSeat(int id, string floor, string area, string seatNumber, string facilities);
    void DeleteSeat(int id);
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

// ▼ 管理端座位列表 DTO
public class AdminSeatListResult
{
    public List<AdminSeatItem> Items { get; set; } = new();
}

public class AdminSeatItem
{
    public int Id { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public SeatStatus Status { get; set; }
    public string Facilities { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
