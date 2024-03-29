﻿using DQueensFashion.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DQueensFashion.Models
{
    public class Cart
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

    public class ViewCartViewModel
    {
        public decimal SubTotal { get; set; }
        public int Count { get; set; }
        public IEnumerable<Cart> Carts { get; set; }
    }
}