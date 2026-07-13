using LibrarySeatReservation.Web.Models.SeedData;
using LibrarySeatReservation.Web.Models.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySeatReservation.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString("StudentName")))
            HttpContext.Session.SetString("StudentName", "学生1");

        var viewModel = new IndexViewModel
        {
            StudentName = HttpContext.Session.GetString("StudentName"),
            Accounts = StudentAccounts.GetAccounts()
        };
        return View(viewModel);
    }

    [HttpPost]
    public IActionResult SwitchAccount(string name)
    {
        if (StudentAccounts.GetAccounts().Contains(name))
        {
            HttpContext.Session.SetString("StudentName", name);
        }
        return RedirectToAction("Index", "Home");
    }
}
