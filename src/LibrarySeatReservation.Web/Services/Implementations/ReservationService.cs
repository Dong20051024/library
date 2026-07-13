using LibrarySeatReservation.Web.Data;
using LibrarySeatReservation.Web.Models.Entities;
using LibrarySeatReservation.Web.Models.Enums;
using LibrarySeatReservation.Web.Models.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace LibrarySeatReservation.Web.Services.Implementations;

public class ReservationService : IReservationService
{
    private readonly AppDbContext _context;

    public ReservationService(AppDbContext context)
    {
        _context = context;
    }

    public void CreateReservation(int seatId, string studentName, DateTime startTime, DateTime endTime)
    {
        // 校验：时间段合理性
        if (startTime >= endTime)
            throw new BusinessException("结束时间必须晚于开始时间");
        if (startTime < DateTime.Now)
            throw new BusinessException("预约开始时间不能早于当前时间");

        using var transaction = _context.Database.BeginTransaction();
        try
        {
            // 校验：座位存在且空闲
            var seat = _context.Seats.FirstOrDefault(s => s.Id == seatId)
                ?? throw new BusinessException("所选座位不存在或已被删除");
            if (!seat.IsActive)
                throw new BusinessException("该座位已禁用，无法预约");
            if (seat.Status != SeatStatus.空闲)
                throw new BusinessException("该座位当前状态无法预约");

            // 校验：时段重叠（核心防重复）
            var conflictCount = _context.Reservations
                .Count(r => r.SeatId == seatId
                         && r.StartTime < endTime
                         && r.EndTime > startTime
                         && (r.Status == ReservationStatus.待签到
                          || r.Status == ReservationStatus.使用中));
            if (conflictCount > 0)
                throw new BusinessException("该座位在您选择的时间段已被预约，请选择其他时段或座位");

            // 写入预约
            var reservation = new Reservation
            {
                SeatId = seatId,
                StudentName = studentName,
                StartTime = startTime,
                EndTime = endTime,
                Status = ReservationStatus.待签到,
                CreatedAt = DateTime.Now
            };
            _context.Reservations.Add(reservation);

            // 更新座位状态
            seat.Status = SeatStatus.已预约;

            _context.SaveChanges();
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public void CancelReservation(int reservationId)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var reservation = _context.Reservations
                .Include(r => r.Seat)
                .FirstOrDefault(r => r.Id == reservationId)
                ?? throw new BusinessException("预约记录不存在");

            if (reservation.Status != ReservationStatus.待签到)
                throw new BusinessException("当前预约状态不允许取消（仅待签到状态可以取消）");

            reservation.Status = ReservationStatus.已取消;
            if (reservation.Seat != null)
                reservation.Seat.Status = SeatStatus.空闲;

            _context.SaveChanges();
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public MyReservationsResult GetMyReservations(string studentName)
    {
        var items = _context.Reservations
            .Include(r => r.Seat)
            .Where(r => r.StudentName == studentName)
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new MyReservationItem
            {
                Id = r.Id,
                SeatNumber = r.Seat != null ? r.Seat.SeatNumber : "",
                Floor = r.Seat != null ? r.Seat.Floor : "",
                Area = r.Seat != null ? r.Seat.Area : "",
                StartTime = r.StartTime,
                EndTime = r.EndTime,
                Status = r.Status
            })
            .ToList();

        return new MyReservationsResult { Items = items };
    }

    public AdminReservationResult GetAllReservations(ReservationStatus? statusFilter)
    {
        var query = _context.Reservations
            .Include(r => r.Seat)
            .AsQueryable();

        if (statusFilter.HasValue)
            query = query.Where(r => r.Status == statusFilter.Value);

        var items = query
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new AdminReservationItemDto
            {
                Id = r.Id,
                SeatNumber = r.Seat != null ? r.Seat.SeatNumber : "",
                Floor = r.Seat != null ? r.Seat.Floor : "",
                Area = r.Seat != null ? r.Seat.Area : "",
                StudentName = r.StudentName,
                StartTime = r.StartTime,
                EndTime = r.EndTime,
                Status = r.Status
            })
            .ToList();

        return new AdminReservationResult
        {
            Items = items,
            CurrentStatus = statusFilter
        };
    }

    public void CheckIn(int reservationId)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var reservation = _context.Reservations
                .Include(r => r.Seat)
                .FirstOrDefault(r => r.Id == reservationId)
                ?? throw new BusinessException("预约记录不存在");

            if (reservation.Status != ReservationStatus.待签到)
                throw new BusinessException("当前预约状态不允许签到（仅待签到状态可签到）");

            reservation.Status = ReservationStatus.使用中;
            if (reservation.Seat != null)
                reservation.Seat.Status = SeatStatus.使用中;

            _context.SaveChanges();
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public void Release(int reservationId)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var reservation = _context.Reservations
                .Include(r => r.Seat)
                .FirstOrDefault(r => r.Id == reservationId)
                ?? throw new BusinessException("预约记录不存在");

            if (reservation.Status != ReservationStatus.使用中)
                throw new BusinessException("当前预约状态不允许释放（仅使用中状态可释放）");

            reservation.Status = ReservationStatus.已完成;
            if (reservation.Seat != null)
                reservation.Seat.Status = SeatStatus.空闲;

            _context.SaveChanges();
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
