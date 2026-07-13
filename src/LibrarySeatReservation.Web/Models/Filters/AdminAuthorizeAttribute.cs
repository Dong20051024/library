using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LibrarySeatReservation.Web.Models.Filters;

public class AdminAuthorizeAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var adminId = context.HttpContext.Session.GetInt32("AdminId");
        if (adminId == null)
        {
            context.Result = new RedirectToActionResult("Index", "Login", new { area = "Admin" });
        }
    }
}
