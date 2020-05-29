using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Core.Model
{
    public class LineItem:Entity
    {
        public decimal TotalAmount { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
        public virtual Order Order { get; set; }
    }
}
