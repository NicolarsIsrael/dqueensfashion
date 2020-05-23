using DQueensFashion.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DQueensFashion.Models
{
    public class AddProductViewModel
    {
        [Required]
        [BeginWIthAlphabeth(ErrorMessage = "Product name must begin with an alphabeth")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Product name must at least be 2 characters long and not more than 50")]
        public string Name { get; set; }

        [Required]
        [BeginWIthAlphabeth(ErrorMessage = "Description must begin with an alphabeth")]
        [StringLength(10000, MinimumLength = 2, ErrorMessage = "Description must at least be 2 characters long and not more than 10000")]
        public string Description { get; set; }

        [Range(0,double.MaxValue,ErrorMessage ="Price must be greater than 0")]
        [Required]
        public decimal Price { get; set; }

        [Range(1,int.MaxValue,ErrorMessage ="Quantity must be greater than 1")]
        [Required]
        public int Quantity { get; set; }

        public string ImagePath1 { get; set; }
        public string ImagePath2 { get; set; }
        public string ImagePath3 { get; set; }
        public string ImagePath4 { get; set; }

        public HttpPostedFileBase ImageFile1 { get; set; }
        public HttpPostedFileBase ImageFile2 { get; set; }
        public HttpPostedFileBase ImageFile3 { get; set; }
        public HttpPostedFileBase ImageFile4 { get; set; }
        [Display(Name ="Category")]
        public int CategoryId { get; set; }
        public IEnumerable<CategoryNameAndId> Categories { get; set; }
    }

    public class ViewProductsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }
        public DateTime DateCreated { get; set; }
        [Display(Name ="Date created")]
        public string DateCreatedString { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Category { get; set; }
    }
}