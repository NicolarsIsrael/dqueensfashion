using DQueensFashion.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DQueensFashion.Models
{
    public class HomeIndexViewModel
    {
        public IEnumerable<ViewProductsViewModel> Products { get; set; }
        public IEnumerable<CategoryNameAndId> Categories { get; set; }
        public IEnumerable<ViewProductsViewModel> BestSellingProducts { get; set; }
        public IEnumerable<ViewProductsViewModel> BestDealsProducts { get; set; }
    }

    public class ProductIndexViewModel
    {
        public IEnumerable<ViewProductsViewModel> Products { get; set; }
        public IEnumerable<CategoryNameAndId> Categories { get; set; }
        public int NumberOfPages { get; set; }
        public int CurrentPageNumber { get; set; }
    }

    public class ContactUsViewModel
    {
        [Required]
        [BeginWIthAlphaNumeric(ErrorMessage = "Fullname name must begin with an alphabeth or number")]
        public string Fullname { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Phone]
        [Required]
        public string Phone { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }
    }
}