using LibrarySeatReservation.Web.Data;
using LibrarySeatReservation.Web.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace LibrarySeatReservation.Web.Services.Implementations;

public class StatisticsService : IStatisticsService
{
    private readonly AppDbContext _context;

    public StatisticsService(AppDbContext context)
    {
        _context = context;
    }

    public StatisticsResult GetStatistics()
    {
        var today = DateTime.Today;

        // 近 7 天每日预约数（以 CreatedAt 日期分组）
        var sevenDaysAgo = today.AddDays(-6);
        var dailyQuery = _context.Reservations
            .Where(r => r.CreatedAt >= sevenDaysAgo)
            .GroupBy(r => r.CreatedAt.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToList();

        var dailyStats = new List<DailyStatItem>();
        for (int i = 0; i < 7; i++)
        {
            var date = sevenDaysAgo.AddDays(i);
            var match = dailyQuery.FirstOrDefault(d => d.Date == date);
            var weekday = date.DayOfWeek switch
            {
                DayOfWeek.Monday => "周一",
                DayOfWeek.Tuesday => "周二",
                DayOfWeek.Wednesday => "周三",
                DayOfWeek.Thursday => "周四",
                DayOfWeek.Friday => "周五",
                DayOfWeek.Saturday => "周六",
                DayOfWeek.Sunday => "周日",
                _ => ""
            };
            dailyStats.Add(new DailyStatItem
            {
                Date = date.ToString("MM/dd"),
                DayLabel = weekday,
                ReservationCount = match?.Count ?? 0
            });
        }

        // 座位状态分布（仅活跃座位）
        var seats = _context.Seats.Where(s => s.IsActive).ToList();
        var distribution = new SeatDistribution
        {
            Free = seats.Count(s => s.Status == SeatStatus.空闲),
            Reserved = seats.Count(s => s.Status == SeatStatus.已预约),
            Occupied = seats.Count(s => s.Status == SeatStatus.使用中)
        };

        // 利用率：当前使用中 / 总活跃座位
        var utilization = distribution.Total > 0
            ? Math.Round((double)distribution.Occupied / distribution.Total * 100, 1)
            : 0.0;

        return new StatisticsResult
        {
            DailyStats = dailyStats,
            SeatDistribution = distribution,
            UtilizationPercentage = utilization
        };
    }
}
