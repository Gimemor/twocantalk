using EMSWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace EMSWeb.Controllers
{
    public class TalkTutorController : Controller
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