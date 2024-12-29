using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
namespace AdminProduct.Models
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext() : base("ProductDbContext") 
        {
        
        }
        // Define your DbSet properties for tables
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Product> Products { get; set; }

    }
    public class Admin
    {
        public int AdminId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
    }
}