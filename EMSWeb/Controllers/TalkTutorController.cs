using Microsoft.AspNetCore.Mvc;

namespace EMSWeb.Controllers
{
    public class TalkTutorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}