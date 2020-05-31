using DQueensFashion.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Core.Model
{
    public class Order:Entity
    {
        public decimal TotalAmount { get; set; }
        public int TotalQuantity { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public List<LineItem> LineItems { get; set; }
        public OrderStatus OrderStatus { get; set; }

    }
}
