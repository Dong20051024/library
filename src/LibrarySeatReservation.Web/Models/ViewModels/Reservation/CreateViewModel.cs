using System.ComponentModel.DataAnnotations;

namespace LibrarySeatReservation.Web.Models.ViewModels.Reservation;

public class CreateViewModel
{
    public int SeatId { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    public DateTime SuggestedStartTime { get; set; }
    public DateTime SuggestedEndTime { get; set; }
    public string? ErrorMessage { get; set; }
}
