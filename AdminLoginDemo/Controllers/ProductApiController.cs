using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AdminLoginDemo.Models;
namespace AdminLoginDemo.Controllers
{
    [RoutePrefix("api/ProductApi")]
    public class ProductApiController : ApiController
    {
        // Static in-memory list for demo 
        // private static List<Product> products = new List<Product>();
        private static List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1", Amount = 100, Description = "Description 1", ImagePath = "image1.jpg" },
            new Product { Id = 2, Name = "Product 2", Amount = 200, Description = "Description 2", ImagePath = "image2.jpg" }
        };
        //POST: api/ProductApi
        //[HttpPost]
        //public IHttpActionResult AddProduct(Product product)
        //{
        //    if(ModelState.IsValid)
        //    {
        //        return BadRequest("Invalid product data");
                
        //    }
        //    // Check for duplicate product
        //    var existingProduct = products.FirstOrDefault(p => p.Name == product.Name);
        //    if (existingProduct != null)
        //    {
        //        return Conflict(); // 409 Conflict if the product already exists
        //    }
        //    product.Id = products.Count + 1;
        //    products.Add(product);
        //    return CreatedAtRoute("DefaultApi", new { id = product.Id }, product); // Return 201 Created
        //}

      
        // GET: api/products
        //[Route("")]
        [HttpGet]
        public IHttpActionResult GetProducts()
        {
            return Ok(products); // Return the list of products in JSON format
            //var pagedProducts = products.Skip((page - 1) * size).Take(size).ToList();
            //return Ok(pagedProducts);
        }
    }
}
