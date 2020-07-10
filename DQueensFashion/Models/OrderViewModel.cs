using DQueensFashion.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public decimal SubTotal { get; set; }
        public decimal ShippingPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public int TotalQuantity { get; set; }
        public string OrderStatus { get; set; }
        public IEnumerable<ViewLineItem> LineItems { get; set; }
        public string LineItemConcatenatedString { get; set; }
    }

    public class ViewLineItem
    {
        public int LineItemId { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalAmount { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public string ProductImage { get; set; }
        public string Description { get; set; }
        public bool CanReview { get; set; }
    }

    public class UpdateOrderStatusViewModel
    {
        public int Id { get; set; }
        [Display(Name ="Order status")]
        public OrderStatus OrderStatus { get; set; }
    }
}

