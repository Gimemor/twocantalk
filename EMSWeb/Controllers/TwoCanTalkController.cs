﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMSWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace EMSWeb.Controllers
{
    public class TwoCanTalkController : Controller
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