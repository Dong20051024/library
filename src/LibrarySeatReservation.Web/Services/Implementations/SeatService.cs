using LibrarySeatReservation.Web.Data;
using LibrarySeatReservation.Web.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibrarySeatReservation.Web.Services.Implementations;

public class SeatService : ISeatService
{
    private readonly AppDbContext _context;

    public SeatService(AppDbContext context)
    {
        _context = context;
    }

    public SeatListResult GetSeatList(string? floor)
    {
        var query = _context.Seats.Where(s => s.IsActive);

        if (!string.IsNullOrEmpty(floor))
            query = query.Where(s => s.Floor == floor);

        var seats = query
            .OrderBy(s => s.Floor)
            .ThenBy(s => s.Area)
            .ThenBy(s => s.SeatNumber)
            .Select(s => new SeatListItem
            {
                Id = s.Id,
                SeatNumber = s.SeatNumber,
                Floor = s.Floor,
                Area = s.Area,
                Status = s.Status,
                Facilities = s.Facilities
            })
            .ToList();

        var floors = _context.Seats
            .Where(s => s.IsActive)
            .Select(s => s.Floor)
            .Distinct()
            .OrderBy(f => f)
            .ToList();

        return new SeatListResult
        {
            Seats = seats,
            CurrentFloor = floor,
            Floors = floors
        };
    }

    public SeatDetailResult? GetSeatDetail(int id)
    {
        var seat = _context.Seats.FirstOrDefault(s => s.Id == id);
        if (seat == null) return null;

        return new SeatDetailResult
        {
            Id = seat.Id,
            SeatNumber = seat.SeatNumber,
            Floor = seat.Floor,
            Area = seat.Area,
            Status = seat.Status,
            Facilities = seat.Facilities,
            IsActive = seat.IsActive
        };
    }
}
