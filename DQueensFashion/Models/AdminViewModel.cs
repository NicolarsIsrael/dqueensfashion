using DQueensFashion.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DQueensFashion.Models
{
    public class AdminViewModel
    {
        public int NumberOfCategories { get; set; }
        public int NumberOfProducts { get; set; }
        public int NumberOfOrders { get; set; }
        public int NumberOfCustomers { get; set; }
    }

    public class GeneralValuesViewModel
    {
        public int GeneralValId { get; set; }

        [Required(ErrorMessage ="Subscription discount is required")]
        [Range(0,100,ErrorMessage ="Subscription discount must be between 0 and 100")]
        [Display(Name ="Subscription discount (%)")]
        public decimal NewsLetterSubscriptionDiscount { get; set; }

        [Required(ErrorMessage ="Shipping price is requirsd")]
        [Range(0,int.MaxValue,ErrorMessage ="Shipping price must be equal to or greater than 0")]
        [Display(Name ="Shipping price")]
        public decimal ShippingPrice { get; set; }

        [Required(ErrorMessage = "Min free shipping is requirsd")]
        [Range(0, int.MaxValue, ErrorMessage = "Min free Shipping price must be equal to or greater than 0")]
        [Display(Name = "Min for free shipping")]
        public decimal MinFreeShippingPrice { get; set; }
    }
}