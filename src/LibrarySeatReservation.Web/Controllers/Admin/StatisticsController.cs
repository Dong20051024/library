using LibrarySeatReservation.Web.Models.Filters;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySeatReservation.Web.Controllers.Admin;

[Area("Admin")]
[Route("Admin/[controller]")]
[AdminAuthorize]
public class StatisticsController : Controller
{
    public IActionResult Index()
    {
        ViewBag.Message = "数据统计 — P2 功能，将在后续 Sprint 中实现";
        return View();
    }
}
