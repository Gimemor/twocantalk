using Microsoft.AspNetCore.Mvc;

namespace EMSWeb.Controllers
{
    public class TextTutorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}