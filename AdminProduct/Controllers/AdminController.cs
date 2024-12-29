using AdminProduct.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
namespace AdminProduct.Controllers
{
    public class AdminController : Controller
    {
        private readonly ProductDbContext _db = new ProductDbContext();

        [HttpGet]
        public ActionResult Login() => View();

        [HttpPost]
        public ActionResult Login(AdminLoginModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = _db.Admins.FirstOrDefault(a => a.Username == model.Username && a.Password == model.Password);
                if (admin != null)
                {
                    Session["AdminId"] = admin.AdminId;
                    return RedirectToAction("CreateProduct");
                }
                ViewBag.Error = "Invalid credentials";
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
        [Authorize] // Ensure only logged-in admins can access
        public ActionResult CreateProduct() => View();

        [HttpPost]
        [Authorize]
        public ActionResult CreateProduct(Product model, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null && Image.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(Image.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                    Image.SaveAs(filePath);
                    model.ImagePath = "/Uploads/" + fileName;
                }

                _db.Products.Add(model);
                _db.SaveChanges();
                return RedirectToAction("ProductList");
            }
            return View(model);
        }
        public ActionResult ProductList(string search, int? page)
        {
            try
            {
                int pageSize = 5;
                int pageNumber = page ?? 1;
                var products = string.IsNullOrWhiteSpace(search)
                    ? _db.Products
                    : _db.Products.Where(p => p.Name.Contains(search));

                var model = new ProductListViewModel
                {
                    Products = products.OrderBy(p => p.Name).ToPagedList(pageNumber, pageSize),
                    SearchQuery = search
                };

                return View(model);
            }
            catch (Exception)
            {
                // Log exception and display a user-friendly error message
                ViewBag.Error = "An error occurred while retrieving the product list.";
                // You can log the exception: e.g., _logger.LogError(ex);
                return View("Error");
            }
        }
        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            var product = _db.Products.Find(id);
            return View(product);
        }

        [HttpPost]
        public ActionResult EditProduct(Product model, HttpPostedFileBase Image)
        {
            var product = _db.Products.Find(model.ProductId);
            if (product != null)
            {
                product.Name = model.Name;
                product.Amount = model.Amount;
                product.Description = model.Description;

                if (Image != null && Image.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(Image.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Uploads"), fileName);
                    Image.SaveAs(filePath);
                    product.ImagePath = "/Uploads/" + fileName;
                }

                _db.SaveChanges();
                return RedirectToAction("ProductList");
            }
            return View(model);
        }


    }
}