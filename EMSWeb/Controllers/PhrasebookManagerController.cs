using Microsoft.AspNetCore.Mvc;

namespace EMSWeb.Controllers
{
    public class PhrasebookManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}