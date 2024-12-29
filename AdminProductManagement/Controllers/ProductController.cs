using AdminProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminProductManagement.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase imageFile)
        {
            if (imageFile != null)
            {
                string path = Server.MapPath("~/Images/") + imageFile.FileName;
                imageFile.SaveAs(path);
                product.ImagePath = "/Images/" + imageFile.FileName;
            }

            // Save product to database (implementation depends on database setup)

            return RedirectToAction("Index");
        }
        public ActionResult Index()
        {
            // Fetch products from database (implementation depends on database setup)
            return View();
        }
        //public ActionResult Index(string search, int page = 1)
        //{
        //    // Fetch products from database (implementation depends on database setup)
        //    int pageSize = 5;
        //    var products =

        //                   .Where(p => string.IsNullOrEmpty(search) || p.Name.Contains(search))
        //                   .Skip((page - 1) * pageSize)
        //                   .Take(pageSize)
        //                   .ToList();

        //    ViewBag.Search = search;
        //    ViewBag.Page = page;
        //    return View(products);
        //}

    }
    

}