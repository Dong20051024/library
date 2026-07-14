using LibrarySeatReservation.Web.Models.Filters;
using LibrarySeatReservation.Web.Models.ViewModels.Admin;
using LibrarySeatReservation.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySeatReservation.Web.Controllers.Admin;

[Area("Admin")]
[AdminAuthorize]
public class StatisticsController : Controller
{
    private readonly IStatisticsService _statisticsService;

    public StatisticsController(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    public IActionResult Index()
    {
        try
        {
            var result = _statisticsService.GetStatistics();
            var viewModel = new StatisticsViewModel
            {
                DailyStats = result.DailyStats,
                FreeSeats = result.SeatDistribution.Free,
                ReservedSeats = result.SeatDistribution.Reserved,
                OccupiedSeats = result.SeatDistribution.Occupied,
                TotalActiveSeats = result.SeatDistribution.Total,
                UtilizationPercentage = result.UtilizationPercentage,
                MaxDailyCount = result.DailyStats.Any()
                    ? result.DailyStats.Max(d => d.ReservationCount)
                    : 0
            };
            return View(viewModel);
        }
        catch
        {
            var viewModel = new StatisticsViewModel { ErrorMessage = "统计数据加载失败，请稍后重试" };
            return View(viewModel);
        }
    }
}
