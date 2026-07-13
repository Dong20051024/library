using LibrarySeatReservation.Web.Data;
using LibrarySeatReservation.Web.Models.Enums;
using LibrarySeatReservation.Web.Models.Exceptions;
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

    // ────────────────── 管理端座位 CRUD ──────────────────

    public AdminSeatListResult GetAllSeats()
    {
        var items = _context.Seats
            .OrderBy(s => s.Floor)
            .ThenBy(s => s.Area)
            .ThenBy(s => s.SeatNumber)
            .Select(s => new AdminSeatItem
            {
                Id = s.Id,
                SeatNumber = s.SeatNumber,
                Floor = s.Floor,
                Area = s.Area,
                Status = s.Status,
                Facilities = s.Facilities,
                IsActive = s.IsActive
            })
            .ToList();

        return new AdminSeatListResult { Items = items };
    }

    public void CreateSeat(string floor, string area, string seatNumber, string facilities)
    {
        // 校验：同一楼层 + 区域 + 座位号 唯一
        var exists = _context.Seats.Any(s =>
            s.Floor == floor && s.Area == area && s.SeatNumber == seatNumber);
        if (exists)
            throw new BusinessException("该楼层该区域已存在相同座位号");

        var seat = new Models.Entities.Seat
        {
            Floor = floor,
            Area = area,
            SeatNumber = seatNumber,
            Facilities = facilities,
            Status = SeatStatus.空闲,
            IsActive = true
        };
        _context.Seats.Add(seat);
        _context.SaveChanges();
    }

    public void UpdateSeat(int id, string floor, string area, string seatNumber, string facilities)
    {
        var seat = _context.Seats.FirstOrDefault(s => s.Id == id)
            ?? throw new BusinessException("座位不存在");

        // 校验唯一性（排除自身）
        var duplicate = _context.Seats.Any(s =>
            s.Id != id
            && s.Floor == floor
            && s.Area == area
            && s.SeatNumber == seatNumber);
        if (duplicate)
            throw new BusinessException("该楼层该区域已存在相同座位号");

        seat.Floor = floor;
        seat.Area = area;
        seat.SeatNumber = seatNumber;
        seat.Facilities = facilities;

        _context.SaveChanges();
    }

    public void DeleteSeat(int id)
    {
        var seat = _context.Seats.FirstOrDefault(s => s.Id == id)
            ?? throw new BusinessException("座位不存在");

        // 检查是否有有效预约（待签到或使用中）
        var hasActiveReservation = _context.Reservations.Any(r =>
            r.SeatId == id
            && (r.Status == ReservationStatus.待签到
             || r.Status == ReservationStatus.使用中));
        if (hasActiveReservation)
            throw new BusinessException("该座位有关联的有效预约记录，无法删除");

        _context.Seats.Remove(seat);
        _context.SaveChanges();
    }
}
