using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EMSWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using Nancy.Session;

namespace EMSWeb.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    public static class SessionHelper
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }
    }
    public class LoginController : Controller
    {
        IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // GET: Login
        public ActionResult Index()
        {
            LoginModel model = new LoginModel();
            model.ErrorMessage = "";
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = ValidateLogin(model.UserName, model.Password);
                if(user==null)
                {
                    model.ErrorMessage = "Username or password is incorrect";
                }
                else
                {
                    SessionHelper.SetObjectAsJson(HttpContext.Session, "userObject", user);
                    //List<WtrStudent> _sessionList = SessionHelper.GetObjectFromJson<List<WtrStudent>>(HttpContext.Session, "userObject");
                    return RedirectToAction("Index", "ResourcesLib");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password))
                {
                    model.ErrorMessage = "Username and Password cannot be blank";
                }
                else
                {
                    model.ErrorMessage = "Please check your Username and Password";
                }
            }
            return View(model);
        }

        private LoggedInUser ValidateLogin(string uname, string pass)
        {
            LoggedInUser user = new LoggedInUser();
            try
            {               
                var conn = _configuration.GetConnectionString("DefaultConnection");
                using (MySqlConnection con = new MySqlConnection(conn))// "Server=localhost; Database=f1_emasuk_devresources; UID=root; PWD=@Mik70525"))
                {
                    using (MySqlCommand cmd = new MySqlCommand("ValidateLogin"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("Uname",uname);
                        cmd.Parameters.AddWithValue("Pass",pass);
                        cmd.Connection = con;
                        con.Open();

                        var d = cmd.ExecuteReader();
                        if (d.HasRows)
                        {
                            while (d.Read())
                            {
                                user.UserName = d["username"].ToString();
                                user.Id = d["id"].ToString();
                                user.OrganisationName = d["organisation_name"].ToString();
                                user.TextTutor = (bool)d["perm_text_tutor"];
                                user.TalkingTutor = (bool)d["perm_talking_tutor"];
                                user.TwoCanTalk = (bool)d["perm_twocan_talk"];
                                user.PhraseBook = (bool)d["perm_phrasebook"];
                                user.UserType = d["type"].ToString();
                            }
                        }
                        else 
                        {
                            return null;
                        }

                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return user;

        }
        // GET: Login/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Login/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Login/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Login/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Login/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Login/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Login/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}