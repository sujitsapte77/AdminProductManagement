using ProductAdmin.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace ProductAdmin.Controllers
{
    [System.Web.Http.RoutePrefix("api/ProductApi")]
    public class ProductApiController : ApiController
    {
        string strcon = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
        
        // GET: api/products
        [System.Web.Http.HttpGet]
        //[Route("")]
      
          
            public IHttpActionResult GetProducts(string search = "", int page = 1, int pageSize = 10)
            {
                // Validate pagination inputs
                if (page <= 0 || pageSize <= 0)
                    return BadRequest("Page and PageSize must be greater than zero.");

                List<Product> products = new List<Product>();
                int totalRecords = 0;

                try
                {
                    using (SqlConnection conn = new SqlConnection(strcon))
                    {
                        conn.Open();

                        // Get total record count
                        string countQuery = "SELECT COUNT(*) FROM Products WHERE Name LIKE @Search";
                        using (SqlCommand countCmd = new SqlCommand(countQuery, conn))
                        {
                            countCmd.Parameters.AddWithValue("@Search", "%" + search + "%");
                            totalRecords = Convert.ToInt32(countCmd.ExecuteScalar());
                        }

                        // Get paginated data
                        string query = @"
                        SELECT * 
                        FROM (
                            SELECT ROW_NUMBER() OVER (ORDER BY ProductId DESC) AS RowNum, * 
                            FROM Products 
                            WHERE Name LIKE @Search
                        ) AS Result 
                        WHERE RowNum BETWEEN @Start AND @End";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@Search", "%" + search + "%");
                            cmd.Parameters.AddWithValue("@Start", (page - 1) * pageSize + 1);
                            cmd.Parameters.AddWithValue("@End", page * pageSize);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
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
                            }
                        }
                    }

                    var response = new
                    {
                        TotalRecords = totalRecords,
                        Products = products
                    };

                    return Ok(response);
                }
                catch (Exception ex)
                {
                    // Log the exception (consider implementing a logging library like NLog or Serilog)
                    return InternalServerError(new Exception("An error occurred while fetching products. Please try again later.", ex));
                }
           

        }
        

    }
}
