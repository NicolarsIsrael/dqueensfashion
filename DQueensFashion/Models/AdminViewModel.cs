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
        [Range(0,100)]
        [Display(Name ="Subscription discount (%)")]
        public decimal NewsLetterSubscriptionDiscount { get; set; }
    }
}