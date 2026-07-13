using LibrarySeatReservation.Web.Models.Enums;
using LibrarySeatReservation.Web.Models.Exceptions;
using LibrarySeatReservation.Web.Models.ViewModels.Reservation;
using LibrarySeatReservation.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySeatReservation.Web.Controllers;

public class ReservationController : Controller
{
    private readonly ISeatService _seatService;
    private readonly IReservationService _reservationService;

    public ReservationController(ISeatService seatService, IReservationService reservationService)
    {
        _seatService = seatService;
        _reservationService = reservationService;
    }

    public IActionResult Create(int seatId)
    {
        try
        {
            var seat = _seatService.GetSeatDetail(seatId);
            if (seat == null)
                return RedirectToAction("NotFound", "Seat");

            if (seat.Status != SeatStatus.空闲)
            {
                var errorVm = new CreateViewModel
                {
                    ErrorMessage = "该座位当前无法预约（当前状态：" + SeatController.GetSeatStatusText(seat.Status) + "）"
                };
                return View(errorVm);
            }

            var suggestedStart = GetNextHourSlot(DateTime.Now);
            var viewModel = new CreateViewModel
            {
                SeatId = seatId,
                SeatNumber = seat.SeatNumber,
                Floor = seat.Floor,
                Area = seat.Area,
                SuggestedStartTime = suggestedStart,
                SuggestedEndTime = suggestedStart.AddHours(3)
            };
            return View(viewModel);
        }
        catch
        {
            return RedirectToAction("List", "Seat");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var studentName = HttpContext.Session.GetString("StudentName") ?? "学生1";
            _reservationService.CreateReservation(
                model.SeatId,
                studentName,
                model.StartTime,
                model.EndTime
            );
            return RedirectToAction(nameof(Mine));
        }
        catch (BusinessException ex)
        {
            model.ErrorMessage = ex.UserMessage;
            return View(model);
        }
        catch
        {
            model.ErrorMessage = "提交失败，请检查网络后重试";
            return View(model);
        }
    }

    public IActionResult Mine()
    {
        try
        {
            var studentName = HttpContext.Session.GetString("StudentName") ?? "学生1";
            var result = _reservationService.GetMyReservations(studentName);
            var viewModel = new MineViewModel
            {
                Items = result.Items.Select(r => new MineItem
                {
                    Id = r.Id,
                    SeatNumber = r.SeatNumber,
                    Floor = r.Floor,
                    Area = r.Area,
                    StartTime = r.StartTime,
                    EndTime = r.EndTime,
                    StatusText = GetReservationStatusText(r.Status),
                    StatusCssClass = GetReservationStatusCss(r.Status),
                    CanCancel = r.Status == ReservationStatus.待签到
                }).ToList()
            };
            return View(viewModel);
        }
        catch
        {
            var viewModel = new MineViewModel { ErrorMessage = "预约记录加载失败，请刷新页面" };
            return View(viewModel);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Cancel(int id)
    {
        try
        {
            var studentName = HttpContext.Session.GetString("StudentName") ?? "学生1";
            _reservationService.CancelReservation(id, studentName);
            TempData["SuccessMessage"] = "预约已取消";
        }
        catch (BusinessException ex)
        {
            TempData["ErrorMessage"] = ex.UserMessage;
        }
        catch
        {
            TempData["ErrorMessage"] = "取消失败，请稍后重试";
        }
        return RedirectToAction(nameof(Mine));
    }

    private static DateTime GetNextHourSlot(DateTime now)
    {
        return new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0).AddHours(1);
    }

    public static string GetReservationStatusText(ReservationStatus status) => status switch
    {
        ReservationStatus.待签到 => "待签到",
        ReservationStatus.使用中 => "使用中",
        ReservationStatus.已完成 => "已完成",
        ReservationStatus.已取消 => "已取消",
        _ => "未知"
    };

    public static string GetReservationStatusCss(ReservationStatus status) => status switch
    {
        ReservationStatus.待签到 => "ls-badge-pending",
        ReservationStatus.使用中 => "ls-badge-occupied",
        ReservationStatus.已完成 => "ls-badge-completed",
        ReservationStatus.已取消 => "ls-badge-cancelled",
        _ => "ls-badge-unknown"
    };
}
