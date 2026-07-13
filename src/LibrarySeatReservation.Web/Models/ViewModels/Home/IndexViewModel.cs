namespace LibrarySeatReservation.Web.Models.ViewModels.Home;

public class IndexViewModel
{
    public string? StudentName { get; set; }
    public string[] Accounts { get; set; } = Array.Empty<string>();
    public string? ErrorMessage { get; set; }
}
