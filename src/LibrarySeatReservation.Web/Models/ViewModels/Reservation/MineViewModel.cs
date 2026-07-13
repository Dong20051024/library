namespace LibrarySeatReservation.Web.Models.ViewModels.Reservation;

public class MineViewModel
{
    public List<MineItem> Items { get; set; } = new();
    public string? ErrorMessage { get; set; }
}

public class MineItem
{
    public int Id { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public string StatusCssClass { get; set; } = string.Empty;
    public bool CanCancel { get; set; }
}
