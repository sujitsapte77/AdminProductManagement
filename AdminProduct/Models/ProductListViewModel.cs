using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminProduct.Models
{
    public class ProductListViewModel
    {
        public PagedList.IPagedList<Product> Products { get; set; }
        public string SearchQuery { get; set; }
    }
}