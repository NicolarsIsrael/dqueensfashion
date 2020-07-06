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

        [Required(ErrorMessage ="Address is required")]
        [BeginWIthAlphaNumeric(ErrorMessage = "Address must begin with an alphabeth")]
        public string Address { get; set; }

        [Required(ErrorMessage ="Email is required")]
        [BeginWIthAlphaNumeric(ErrorMessage = "Email must begin with an alphabeth")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Phone number is requiered")]
        [Phone]
        [Display(Name ="Phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage ="Subscription discount is required")]
        [Range(0,100)]
        [Display(Name ="Subscription discount (%)")]
        public decimal NewsLetterSubscriptionDiscount { get; set; }
    }
}