using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Core.Model
{
    public class WishList: Entity
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImagePath { get; set; }
        public decimal ProductPrice { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}
