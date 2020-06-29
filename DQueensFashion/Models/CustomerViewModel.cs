using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DQueensFashion.Models
{
    public class CustomerViewModel
    {
        public int CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerFullName { get; set; }
        public int TotalCustomerOrders { get; set; }
        public int TotalCustomerWishList { get; set; }
    }
}