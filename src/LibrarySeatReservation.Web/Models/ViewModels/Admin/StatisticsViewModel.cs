using LibrarySeatReservation.Web.Services;

namespace LibrarySeatReservation.Web.Models.ViewModels.Admin;

public class StatisticsViewModel
{
    public string? ErrorMessage { get; set; }

    // 近 7 天每日预约数
    public List<DailyStatItem> DailyStats { get; set; } = new();

    // 座位状态分布
    public int FreeSeats { get; set; }
    public int ReservedSeats { get; set; }
    public int OccupiedSeats { get; set; }
    public int TotalActiveSeats { get; set; }

    // 利用率
    public double UtilizationPercentage { get; set; }

    // 最大预约数（用于柱状图比例）
    public int MaxDailyCount { get; set; }
}
