﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductAdmin.Models
{
  
        public class Product
        {
            public int ProductId { get; set; }
            public string Name { get; set; }
            public decimal Amount { get; set; }
            public string Description { get; set; }
            public string ImagePath { get; set; }
        }

    
}