using EMSWeb.Filters;
using EMSWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace EMSWeb.Controllers
{
    [ClaimRequirement(ClaimType.TextTutor)]
    public class TextTutorController : Controller
    {
        public IActionResult Index()
        {
            LoggedInUser userSession = SessionHelper.GetObjectFromJson<LoggedInUser>(HttpContext.Session, "userObject");
            if (userSession is null)
            {
                RedirectToAction("Index", "Login");
            }
            return View();
        }
    }
}