namespace LibrarySeatReservation.Web.Models.ViewModels.Seat;

public class DetailViewModel
{
    public int Id { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string StatusText { get; set; } = string.Empty;
    public string StatusCssClass { get; set; } = string.Empty;
    public string Facilities { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool IsAvailable { get; set; }
    public string? ErrorMessage { get; set; }
}
