using LibrarySeatReservation.Web.Models.Enums;
using LibrarySeatReservation.Web.Models.ViewModels.Seat;
using LibrarySeatReservation.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySeatReservation.Web.Controllers;

public class SeatController : Controller
{
    private readonly ISeatService _seatService;

    public SeatController(ISeatService seatService)
    {
        _seatService = seatService;
    }

    public IActionResult List(string? floor)
    {
        try
        {
            var result = _seatService.GetSeatList(floor);
            var viewModel = new ListViewModel
            {
                Seats = result.Seats.Select(s => new SeatItem
                {
                    Id = s.Id,
                    SeatNumber = s.SeatNumber,
                    Floor = s.Floor,
                    Area = s.Area,
                    StatusText = GetSeatStatusText(s.Status),
                    StatusCssClass = GetSeatStatusCss(s.Status),
                    Facilities = s.Facilities
                }).ToList(),
                CurrentFloor = result.CurrentFloor,
                Floors = result.Floors
            };
            return View(viewModel);
        }
        catch
        {
            var viewModel = new ListViewModel { ErrorMessage = "座位信息加载失败，请刷新页面重试" };
            return View(viewModel);
        }
    }

    public IActionResult Detail(int id)
    {
        try
        {
            var result = _seatService.GetSeatDetail(id);
            if (result == null)
                return View("NotFound");

            var viewModel = new DetailViewModel
            {
                Id = result.Id,
                SeatNumber = result.SeatNumber,
                Floor = result.Floor,
                Area = result.Area,
                StatusText = GetSeatStatusText(result.Status),
                StatusCssClass = GetSeatStatusCss(result.Status),
                Facilities = result.Facilities,
                IsActive = result.IsActive,
                IsAvailable = result.IsActive && result.Status == SeatStatus.空闲
            };
            return View(viewModel);
        }
        catch
        {
            var viewModel = new DetailViewModel { ErrorMessage = "座位信息加载失败" };
            return View(viewModel);
        }
    }

    public static string GetSeatStatusText(SeatStatus status) => status switch
    {
        SeatStatus.空闲 => "空闲",
        SeatStatus.已预约 => "已预约",
        SeatStatus.使用中 => "使用中",
        _ => "未知"
    };

    public static string GetSeatStatusCss(SeatStatus status) => status switch
    {
        SeatStatus.空闲 => "ls-badge-free",
        SeatStatus.已预约 => "ls-badge-reserved",
        SeatStatus.使用中 => "ls-badge-occupied",
        _ => "ls-badge-unknown"
    };
}
