using AdminLoginDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminLoginDemo.Controllers
{
    public class AccountController : Controller
    {
        private const string AdminUsername = "admin";
        private const string AdminPassword = "admin123";
        // GET: Login page
        public ActionResult Login()
        {
            return View();
        }
        //POST: Handle Login
        [HttpPost]
        public ActionResult Login(Admin admin)
        {
            if(admin.username== AdminUsername && admin.password==AdminPassword)
            {
                Session["username"] = admin.username;
                return RedirectToAction("Create", "Product");
            }
            ViewBag.Message = "Invalid Username or password";
            return View();
        }
        //Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}