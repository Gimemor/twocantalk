using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMSWeb.BusinessServices.Services.Interfaces;
using EMSWeb.Filters;
using EMSWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace EMSWeb.Controllers
{
    [ClaimRequirement(ClaimType.UserManagement)]
    public class UserManagementController : Controller
    {
        protected IUserManagementService _userManagementService;

        public UserManagementController(IUserManagementService service)
        {
            _userManagementService = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            LoggedInUser userSession = SessionHelper.GetObjectFromJson<LoggedInUser>(HttpContext.Session, "userObject");
            if (userSession is null)
            {
                RedirectToAction("Index", "Login");
            }
            return View();
        }


        // GET: api/<controller>
        [HttpGet]
        public async Task<PagingResult<User>> Get(Paging paging)
        {
            return await _userManagementService.GetList(paging);
        }

        [HttpGet]
        [Route("UserManagement/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _userManagementService.Get(id);
            if(model == null) 
            {
                return NotFound();
            }
            return View("Edit", model);
        }

        [HttpDelete]
        [Route("UserManagement/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userManagementService.Delete(id);
            return Ok();
        }

        [HttpGet]
        [Route("UserManagement/Add")]
        public IActionResult Add()
        {
            return View("Edit", new User
            {
                Id = 0
            });
        }

        [HttpPost]
        [Route("UserManagement/Save")]
        public async Task<IActionResult> Save(User user) 
        {
            if (user.Id == 0)
            {
                await _userManagementService.Insert(user);
            }
            else 
            {
                await _userManagementService.Update(user);
            }
            return RedirectToAction("Index");
        }
    }
}