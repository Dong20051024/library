using LibrarySeatReservation.Web.Models.Enums;
using LibrarySeatReservation.Web.Models.Exceptions;
using LibrarySeatReservation.Web.Models.Filters;
using LibrarySeatReservation.Web.Models.ViewModels.Admin;
using LibrarySeatReservation.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySeatReservation.Web.Controllers.Admin;

[Area("Admin")]
[AdminAuthorize]
public class SeatController : Controller
{
    private readonly ISeatService _seatService;

    public SeatController(ISeatService seatService)
    {
        _seatService = seatService;
    }

    public IActionResult Index()
    {
        try
        {
            var result = _seatService.GetAllSeats();
            var viewModel = new SeatViewModel
            {
                Items = result.Items.Select(s => new AdminSeatViewItem
                {
                    Id = s.Id,
                    SeatNumber = s.SeatNumber,
                    Floor = s.Floor,
                    Area = s.Area,
                    Status = s.Status,
                    StatusText = GetSeatStatusText(s.Status),
                    StatusCssClass = GetSeatStatusCss(s.Status),
                    Facilities = s.Facilities,
                    IsActive = s.IsActive,
                    IsDeletable = s.IsActive
                }).ToList()
            };
            return View(viewModel);
        }
        catch
        {
            var viewModel = new SeatViewModel { ErrorMessage = "座位列表加载失败，请刷新后重试" };
            return View(viewModel);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(AdminSeatCreateModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Floor)
            || string.IsNullOrWhiteSpace(model.Area)
            || string.IsNullOrWhiteSpace(model.SeatNumber))
        {
            TempData["ErrorMessage"] = "楼层、区域和座位号为必填项";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            _seatService.CreateSeat(model.Floor.Trim(), model.Area.Trim(),
                model.SeatNumber.Trim(), model.Facilities?.Trim() ?? "");
            TempData["SuccessMessage"] = $"座位 {model.SeatNumber} 新增成功";
        }
        catch (BusinessException ex)
        {
            TempData["ErrorMessage"] = ex.UserMessage;
        }
        catch
        {
            TempData["ErrorMessage"] = "新增失败，请检查填写内容";
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(AdminSeatEditModel model)
    {
        if (string.IsNullOrWhiteSpace(model.Floor)
            || string.IsNullOrWhiteSpace(model.Area)
            || string.IsNullOrWhiteSpace(model.SeatNumber))
        {
            TempData["ErrorMessage"] = "楼层、区域和座位号为必填项";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            _seatService.UpdateSeat(model.Id, model.Floor.Trim(), model.Area.Trim(),
                model.SeatNumber.Trim(), model.Facilities?.Trim() ?? "");
            TempData["SuccessMessage"] = $"座位 {model.SeatNumber} 更新成功";
        }
        catch (BusinessException ex)
        {
            TempData["ErrorMessage"] = ex.UserMessage;
        }
        catch
        {
            TempData["ErrorMessage"] = "更新失败，请重试";
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        try
        {
            _seatService.DeleteSeat(id);
            TempData["SuccessMessage"] = "座位已删除";
        }
        catch (BusinessException ex)
        {
            TempData["ErrorMessage"] = ex.UserMessage;
        }
        catch
        {
            TempData["ErrorMessage"] = "删除失败，该座位可能有关联数据";
        }
        return RedirectToAction(nameof(Index));
    }

    private static string GetSeatStatusText(SeatStatus status) => status switch
    {
        SeatStatus.空闲 => "空闲",
        SeatStatus.已预约 => "已预约",
        SeatStatus.使用中 => "使用中",
        _ => "未知"
    };

    private static string GetSeatStatusCss(SeatStatus status) => status switch
    {
        SeatStatus.空闲 => "ls-badge-free",
        SeatStatus.已预约 => "ls-badge-reserved",
        SeatStatus.使用中 => "ls-badge-occupied",
        _ => "bg-secondary"
    };
}
