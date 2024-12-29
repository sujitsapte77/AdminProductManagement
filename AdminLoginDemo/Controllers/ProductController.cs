using AdminLoginDemo.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
//using PagedList;
namespace AdminLoginDemo.Controllers
{
    public class ProductController : Controller
    {
        // GET: Welcome Product page
        public ActionResult Welcome()
        {
            if (Session["username"] ==null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.Username=Session["username"];
            return View();
        }
        //GET: Create Product Page
        public ActionResult Create()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.Username = Session["username"];
            return View();
        }
        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase imageFile)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                // Save image if provided
                if (imageFile != null)
                {
                    string imagePath = "~/UploadedImages/" + imageFile.FileName;
                    imageFile.SaveAs(Server.MapPath(imagePath));
                    product.ImagePath = imagePath;
                }

                // Add product to the list
                product.Id = products.Count + 1; // Simple auto-increment ID
                products.Add(product);

                TempData["SuccessMessage"] = "Product added successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to add product. Please try again.";
            }

            return RedirectToAction("List");
        }
        // Temporary in-memory list of products for demonstration
        private static List<Product> products = new List<Product>();
        // GET: List Products
        public ActionResult List(string search, int? page)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Filter products based on search query
            var filteredProducts = string.IsNullOrEmpty(search)
                ? products
                : products.Where(p => p.Name.ToLower().Contains(search.ToLower()) ||
                                      p.Description.ToLower().Contains(search.ToLower())).ToList();

            // Paginate results
            int pageSize = 5; // Items per page
            int pageNumber = page ?? 1; // Current page
            return View(filteredProducts.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Edit(int Id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var product = products.FirstOrDefault(p => p.Id == Id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Product not found.";
                return RedirectToAction("List");
            }
            return View(product);

        }
        [HttpPost]
        public ActionResult Edit(Product updatedProduct, HttpPostedFileBase imageFile) 
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var product = products.FirstOrDefault(p => p.Id == updatedProduct.Id);
            if (product == null)
            {
                TempData["ErrorMessage"] = "Product not found.";
                return RedirectToAction("List");
            }
            if (ModelState.IsValid)
            {
                // Update product details
                product.Name = updatedProduct.Name;
                product.Amount = updatedProduct.Amount;
                product.Description = updatedProduct.Description;

                // Update the image if a new file is provided
                if (imageFile != null)
                {
                    string imagePath = "~/UploadedImages/" + imageFile.FileName;
                    imageFile.SaveAs(Server.MapPath(imagePath));
                    product.ImagePath = imagePath;
                }

                TempData["SuccessMessage"] = "Product updated successfully!";
                return RedirectToAction("List");
            }

            TempData["ErrorMessage"] = "Failed to update product. Please try again.";
            return View(updatedProduct);
        }
        
    }
}

   