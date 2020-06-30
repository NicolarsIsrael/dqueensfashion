using DQueensFashion.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DQueensFashion.Models
{
    public class AddReviewViewModel
    {
        public int ProductId { get; set; }
        public int LineItemId { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public string ProductImage { get; set; }
        public string ProductPrice { get; set; }
        public string ProductSubTotal { get; set; }
        public RatingViewModel ProductAverageRating { get; set; }

        [Required]
        public string Comment { get; set; }

        [Range(1,5,ErrorMessage ="Rating should be between 1 and 5")]
        public int Rating { get; set; }

        [Required]
        [BeginWIthAlphaNumeric(ErrorMessage ="Name should begin with letter or number")]
        public string Name { get; set; }

    }

    public class ViewReviewViewModel
    {
        public int ReviewId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string DateCreated { get; set; }
        public DateTime DateOrder { get; set; }
    }

    public class RatingViewModel
    {
        public string AverageRating { get; set; }
        public string TotalReviewCount { get; set; }
        public int FloorAverageRating { get; set; }
        public bool IsDouble { get; set; }
    }
}