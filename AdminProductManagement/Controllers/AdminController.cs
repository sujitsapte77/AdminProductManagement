using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminProductManagement.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password) 
        {
            if (username == "admin" || password == "password")
            {
                return RedirectToAction("Dashboard");
            }
            ViewBag.ErrorMessage = "Invalid Credentials";
            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}