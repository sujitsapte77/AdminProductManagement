using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AdminProduct.Models;

namespace AdminProduct.Controllers
{
    public class ProductsApiController : ApiController
    {
        private readonly ProductDbContext _db = new ProductDbContext();

        [HttpGet]
        public IHttpActionResult GetProducts()
        {
            var products = _db.Products.Select(p => new
            {
                p.ProductId,
                p.Name,
                p.Amount,
                p.Description,
                p.ImagePath
            }).ToList();

            return Ok(products);
        }
    }
}
