using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EMSWeb.Controllers
{
    public class TwoCanTalkController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}