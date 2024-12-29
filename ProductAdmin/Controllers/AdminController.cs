using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProductAdmin.Models;

namespace ProductAdmin.Controllers
{
    
    public class AdminController : Controller
    {
        string strcon = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;

        // GET: Admin
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
       
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(strcon))
            {
                string query = "SELECT COUNT(1) FROM Admins WHERE Username = @Username AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password); // Use hashing for passwords

                conn.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();

                if (count == 1)
                {
                    Session["Admin"] = username;
                    return RedirectToAction("CreateProduct");
                }
                ViewBag.Message = "Invalid login credentials!";
            }
            return View();
        }
        [HttpGet]
        public ActionResult CreateProduct()
        {
            if (Session["Admin"] == null)
                return RedirectToAction("Login");

            return View();
        }

        [HttpPost]
        public ActionResult CreateProduct(string name, decimal amount, string description, HttpPostedFileBase image)
        {
            // Check if the user is logged in
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            // Validate if image is selected
            if (image == null || image.ContentLength == 0)
            {
                ViewBag.ErrorMessage = "Please select an image.";
                return View();
            }
            
            string imagePath = null;
            if (image != null)
            {
                string fileName = Path.GetFileName(image.FileName);
                string path = Server.MapPath("~/Images/" + fileName);
                image.SaveAs(path);
                imagePath = "/Images/" + fileName;
            }

            using (SqlConnection conn = new SqlConnection(strcon))
            {
                string query = "INSERT INTO Products (Name, Amount, Description, ImagePath) VALUES (@Name, @Amount, @Description, @ImagePath)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Amount", amount);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@ImagePath", imagePath);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return RedirectToAction("ProductList");
        }

        // GET: Edit Product
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Product product = null;
            using (SqlConnection conn = new SqlConnection(strcon))
            {
                string query = "SELECT * FROM Products WHERE ProductId = @ProductId";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProductId", id);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    product = new Product
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        Name = reader["Name"].ToString(),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        Description = reader["Description"].ToString(),
                        ImagePath = reader["ImagePath"].ToString()
                    };
                }
                conn.Close();
            }
            return View(product);
        }
        // POST: Edit Product
        [HttpPost]
        public ActionResult EditProduct(int id, string name, decimal amount, string description, HttpPostedFileBase image)
        {
            string imagePath = null;

            // Save image if uploaded
            if (image != null)
            {
                string fileName = Path.GetFileName(image.FileName);
                string path = Server.MapPath("~/Images/" + fileName);
                image.SaveAs(path);
                imagePath = "/Images/" + fileName;
            }

            using (SqlConnection conn = new SqlConnection(strcon))
            {
                string query;

                // Update query based on whether an image is uploaded
                if (imagePath == null)
                {
                    query = "UPDATE Products SET Name = @Name, Amount = @Amount, Description = @Description WHERE ProductId = @ProductId";
                }
                else
                {
                    query = "UPDATE Products SET Name = @Name, Amount = @Amount, Description = @Description, ImagePath = @ImagePath WHERE ProductId = @ProductId";
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Amount", amount);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@ProductId", id);

                if (imagePath != null)
                {
                    cmd.Parameters.AddWithValue("@ImagePath", imagePath);
                }

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "An error occurred: " + ex.Message;
                }
                finally
                {
                    conn.Close();
                }
            }

            return RedirectToAction("ProductList");
        }
      
        public ActionResult ProductList(string search, int page = 1, int pageSize = 5)
        {
            // Check if the user is logged in
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            List<Product> products = new List<Product>();
            int totalRecords = 0;

            using (SqlConnection conn = new SqlConnection(strcon))
            {
                string countQuery = "SELECT COUNT(*) FROM Products WHERE Name LIKE @Search";
                string query = "SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY ProductId DESC) AS RowNum, * FROM Products WHERE Name LIKE @Search) AS Result WHERE RowNum BETWEEN @Start AND @End";

                SqlCommand countCmd = new SqlCommand(countQuery, conn);
                SqlCommand cmd = new SqlCommand(query, conn);
                countCmd.Parameters.AddWithValue("@Search", "%" + search + "%");
                cmd.Parameters.AddWithValue("@Search", "%" + search + "%");
                cmd.Parameters.AddWithValue("@Start", (page - 1) * pageSize + 1);
                cmd.Parameters.AddWithValue("@End", page * pageSize);

                conn.Open();
                totalRecords = Convert.ToInt32(countCmd.ExecuteScalar());
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        Name = reader["Name"].ToString(),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        Description = reader["Description"].ToString(),
                        ImagePath = reader["ImagePath"].ToString()
                    });
                }
                conn.Close();
            }

            ViewBag.TotalRecords = totalRecords;
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            return View(products);
        }
        // Logout Action
        public ActionResult Logout()
        {
            Session["Admin"] = null; // Clear session
            return RedirectToAction("Login"); // Redirect to login page
        }
        [HttpPost]
       // [ValidateAntiForgeryToken]
       
        public ActionResult DeleteProduct(int id)
        {
            if (Session["Admin"] == null)
            {
                return RedirectToAction("Login", "Admin");  // Redirect to login if not logged in
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(strcon))
                {
                    string query = "DELETE FROM Products WHERE ProductId = @ProductId";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ProductId", id);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();

                    if (rowsAffected > 0)
                    {
                        // Optionally, display a success message
                        TempData["SuccessMessage"] = "Product deleted successfully!";
                    }
                    else
                    {
                        // Optionally, display an error message
                        TempData["ErrorMessage"] = "Product not found!";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred: " + ex.Message;
            }

            return RedirectToAction("ProductList");  // Redirect back to the ProductList page after deletion
        }



    }
}