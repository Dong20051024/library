using LibrarySeatReservation.Web.Models.Entities;

namespace LibrarySeatReservation.Web.Services;

public interface IAdminService
{
    Admin? Validate(string username, string password);
}
