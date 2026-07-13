namespace LibrarySeatReservation.Web.Models.ViewModels.Seat;

public class ListViewModel
{
    public List<SeatItem> Seats { get; set; } = new();
    public string? CurrentFloor { get; set; }
    public List<string> Floors { get; set; } = new();
    public string? ErrorMessage { get; set; }
}

public class SeatItem
{
    public int Id { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string StatusText { get; set; } = string.Empty;
    public string StatusCssClass { get; set; } = string.Empty;
    public string Facilities { get; set; } = string.Empty;
}
