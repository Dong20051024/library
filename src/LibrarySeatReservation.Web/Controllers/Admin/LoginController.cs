using LibrarySeatReservation.Web.Models.ViewModels.Admin;
using LibrarySeatReservation.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySeatReservation.Web.Controllers.Admin;

[Area("Admin")]
public class LoginController : Controller
{
    private readonly IAdminService _adminService;

    public LoginController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        if (HttpContext.Session.GetInt32("AdminId") != null)
            return RedirectToAction("Index", "Reservation", new { area = "Admin" });

        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var admin = _adminService.Validate(model.Username, model.Password);
            if (admin == null)
            {
                model.ErrorMessage = "账号或密码错误，请重新输入";
                return View(model);
            }

            HttpContext.Session.SetInt32("AdminId", admin.Id);
            HttpContext.Session.SetString("AdminDisplayName", admin.DisplayName);

            return RedirectToAction("Index", "Reservation", new { area = "Admin" });
        }
        catch
        {
            model.ErrorMessage = "登录失败，请稍后重试";
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
}
