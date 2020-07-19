using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DQueensFashion.Models
{
    public class ViewWishListViewModel
    {
        public int WishListId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImagePath { get; set; }
        public decimal ProductPrice { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public string GeneratedUrl { get; set; }
        public bool IsOutOfStock { get; set; }
    }
}