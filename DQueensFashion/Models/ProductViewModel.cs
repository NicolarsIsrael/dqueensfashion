using DQueensFashion.Core;
using DQueensFashion.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DQueensFashion.Models
{

    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required]
        [BeginWIthAlphaNumeric(ErrorMessage = "Product name must begin with an alphabeth")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Product name must at least be 2 characters long and not more than 50")]
        public string Name { get; set; }

        [Required]
        [BeginWIthAlphaNumeric(ErrorMessage = "Description must begin with an alphabeth")]
        [StringLength(10000, MinimumLength = 2, ErrorMessage = "Description must at least be 2 characters long and not more than 10000")]
        public string Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.01")]
        [Required]
        public decimal Price { get; set; }

        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100")]
        [Display(Name = "Discount(%)")]
        public decimal Discount { get; set; }

        public decimal SubTotal { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 1")]
        [Required]
        public int Quantity { get; set; }

        public List<string> Tags { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public IEnumerable<CategoryNameAndId> Categories { get; set; }

        public bool CustomMadeCategory { get; set; }
        public bool ReadyMadeCategory { get; set; }


        //measurement
        public bool Shoulder { get; set; }
        [Display(Name = "Arm hole")]
        public bool ArmHole { get; set; }
        public bool Bust { get; set; }
        public bool Waist { get; set; }
        public bool Hips { get; set; }
        public bool Thigh { get; set; }
        [Display(Name = "Full body length")]
        public bool FullBodyLength { get; set; }
        [Display(Name = "Knee garment length")]
        public bool KneeGarmentLength { get; set; }

        [Display(Name = "Top length")]
        public bool TopLength { get; set; }

        [Display(Name = "Trouser length")]
        public bool TrousersLength { get; set; }

        [Display(Name = "Round ankle")]
        public bool RoundAnkle { get; set; }

        [Display(Name = "Nip nip")]
        public bool NipNip { get; set; }

        [Display(Name = "Sleeve length")]
        public bool SleeveLength { get; set; }
    }


    public class AddProductImageViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
    }

    public class EditProductImageViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public IEnumerable<ImageViewModel> ProductImages { get; set; }
    }

    public class ImageViewModel
    {
        public string ImagePath { get; set; }
        public int Id { get; set; }
        public int ProductId { get; set; }
    }

    public class ViewProductsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public int Quantity { get; set; }
        public DateTime DateCreated { get; set; }
        [Display(Name ="Date created")]
        public string DateCreatedString { get; set; }
        public List<string> OtherImagePaths { get; set; }
        public string MainImage { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public string Tags { get; set; }
        public int NumberOfOrders { get; set; }
        public RatingViewModel Rating { get; set; }
        public IEnumerable<ViewReviewViewModel> Reviews { get; set; }

        //measurement
        public bool Shoulder { get; set; }
        [Display(Name = "Arm hole")]
        public bool ArmHole { get; set; }
        public bool Bust { get; set; }
        public bool Waist { get; set; }
        public bool Hips { get; set; }
        public bool Thigh { get; set; }
        [Display(Name = "Full body length")]
        public bool FullBodyLength { get; set; }
        [Display(Name = "Knee garment length")]
        public bool KneeGarmentLength { get; set; }

        [Display(Name = "Top length")]
        public bool TopLength { get; set; }

        [Display(Name = "Trouser length")]
        public bool TrousersLength { get; set; }

        [Display(Name = "Round ankle")]
        public bool RoundAnkle { get; set; }

        [Display(Name = "Nip nip")]
        public bool NipNip { get; set; }

        [Display(Name = "Sleeve length")]
        public bool SleeveLength { get; set; }

    }

    public class ProductDetailsViewModel
    {
        public ViewProductsViewModel Product { get; set; }
        public IEnumerable<ViewProductsViewModel> RelatedProducts { get; set; }
    }
}

// shoulder
//arm hole
//Bust
//waist
//hips
//thigh
//full body length
//kneel garment length
//top length
//trousers length
//round ankle
//nip - nip
//sleeves length