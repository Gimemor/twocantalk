using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using EMSWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;

namespace EMSWeb.Controllers
{
    public class LoginController : Controller
    {
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
                string ubane = ValidateLogin(model.UserName, model.Password);
                if(string.IsNullOrEmpty(ubane))
                {
                    model.ErrorMessage = "Username or password is incorrect";
                }
                else
                {
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

        private string ValidateLogin(string uname, string pass)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection("Server=localhost; Database=emsdb; UID=root; PWD=Mik70525"))
                {
                    using (MySqlCommand cmd = new MySqlCommand($"SELECT username  FROM users where username = '{uname}' and password_plain = '{pass}';"))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();

                        var d = cmd.ExecuteReader();
                        if (d.HasRows)
                        {
                            while (d.Read())
                            {
                                return d["username"].ToString();
                            }
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return string.Empty;

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