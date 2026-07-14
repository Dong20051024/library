using LibrarySeatReservation.Web.Models.Enums;
using LibrarySeatReservation.Web.Models.Exceptions;
using LibrarySeatReservation.Web.Models.Filters;
using LibrarySeatReservation.Web.Models.ViewModels.Admin;
using LibrarySeatReservation.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySeatReservation.Web.Controllers.Admin;

[Area("Admin")]
[AdminAuthorize]
public class ReservationController : Controller
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    public IActionResult Index(ReservationStatus? status)
    {
        try
        {
            var result = _reservationService.GetAllReservations(status);
            var viewModel = new ReservationViewModel
            {
                Items = result.Items.Select(r => new AdminReservationItem
                {
                    Id = r.Id,
                    SeatNumber = r.SeatNumber,
                    Floor = r.Floor,
                    Area = r.Area,
                    StudentName = r.StudentName,
                    StartTime = r.StartTime,
                    EndTime = r.EndTime,
                    StatusText = GetStatusText(r.Status),
                    StatusCssClass = GetStatusCss(r.Status),
                    CanCheckIn = r.Status == ReservationStatus.待签到,
                    CanRelease = r.Status == ReservationStatus.使用中
                }).ToList(),
                CurrentStatus = status
            };
            return View(viewModel);
        }
        catch
        {
            var viewModel = new ReservationViewModel { ErrorMessage = "预约记录加载失败" };
            return View(viewModel);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CheckIn(int id)
    {
        try
        {
            _reservationService.CheckIn(id);
            TempData["SuccessMessage"] = "签到成功";
        }
        catch (BusinessException ex)
        {
            TempData["ErrorMessage"] = ex.UserMessage;
        }
        catch
        {
            TempData["ErrorMessage"] = "签到失败，请刷新后重试";
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Release(int id)
    {
        try
        {
            _reservationService.Release(id);
            TempData["SuccessMessage"] = "座位已释放";
        }
        catch (BusinessException ex)
        {
            TempData["ErrorMessage"] = ex.UserMessage;
        }
        catch
        {
            TempData["ErrorMessage"] = "释放失败，请刷新后重试";
        }
        return RedirectToAction(nameof(Index));
    }

    private static string GetStatusText(ReservationStatus status) => status switch
    {
        ReservationStatus.待签到 => "待签到",
        ReservationStatus.使用中 => "使用中",
        ReservationStatus.已完成 => "已完成",
        ReservationStatus.已取消 => "已取消",
        _ => "未知"
    };

    private static string GetStatusCss(ReservationStatus status) => status switch
    {
        ReservationStatus.待签到 => "ls-badge-pending",
        ReservationStatus.使用中 => "ls-badge-occupied",
        ReservationStatus.已完成 => "ls-badge-completed",
        ReservationStatus.已取消 => "ls-badge-cancelled",
        _ => "ls-badge-unknown"
    };
}
