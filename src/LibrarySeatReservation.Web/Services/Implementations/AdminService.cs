using LibrarySeatReservation.Web.Data;
using LibrarySeatReservation.Web.Models.Entities;

namespace LibrarySeatReservation.Web.Services.Implementations;

public class AdminService : IAdminService
{
    private readonly AppDbContext _context;

    public AdminService(AppDbContext context)
    {
        _context = context;
    }

    public Admin? Validate(string username, string password)
    {
        return _context.Admins
            .FirstOrDefault(a => a.Username == username && a.Password == password);
    }
}
