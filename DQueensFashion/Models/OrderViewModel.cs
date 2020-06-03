using DQueensFashion.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DQueensFashion.Models
{
    public class ViewOrderViewModel
    {
        public int OrderId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string DateCreatedString { get; set; }
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalQuantity { get; set; }
        public string OrderStatus { get; set; }
        public IEnumerable<ViewLineItem> LineItems { get; set; }
        public string LineItemConcatenatedString { get; set; }
    }

    public class ViewLineItem
    {
        public decimal TotalAmount { get; set; }
        public int Quantity { get; set; }
        public string Product { get; set; }
    }

    public class UpdateOrderStatusViewModel
    {
        public int Id { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}

