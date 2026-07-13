using LibrarySeatReservation.Web.Models.Enums;

namespace LibrarySeatReservation.Web.Models.ViewModels.Admin;

public class SeatViewModel
{
    public List<AdminSeatViewItem> Items { get; set; } = new();
    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }
}

public class AdminSeatViewItem
{
    public int Id { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public SeatStatus Status { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public string StatusCssClass { get; set; } = string.Empty;
    public string Facilities { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool IsDeletable { get; set; }
}

public class AdminSeatCreateModel
{
    public string Floor { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string SeatNumber { get; set; } = string.Empty;
    public string Facilities { get; set; } = string.Empty;
}

public class AdminSeatEditModel
{
    public int Id { get; set; }
    public string Floor { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string SeatNumber { get; set; } = string.Empty;
    public string Facilities { get; set; } = string.Empty;
}

public class AdminSeatDeleteModel
{
    public int Id { get; set; }
}
