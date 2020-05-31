using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DQueensFashion.Models
{
    public class HomeIndexViewModel
    {
        public IEnumerable<ViewProductsViewModel> Products { get; set; }
    }

    public class ProductIndexViewModel
    {
        public IEnumerable<ViewProductsViewModel> Products { get; set; }
        public IEnumerable<CategoryNameAndId> Categories { get; set; }
    }
}