using DQueensFashion.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DQueensFashion.Models
{
    public class Cart
    {
        public Product Product { get; set; }
        public string MainImage { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal InitialPrice { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string Description { get; set; }

        //measurement
        [Display(Name = "Waist length")]
        public bool WaistLength { get; set; }
        [Display(Name = "Burst size")]
        public bool BurstSize { get; set; }
        [Display(Name = "Shoulder length")]
        public bool ShoulderLength { get; set; }

        public int BurstSizeValue { get; set; }
        public int ShoulderLengthValue { get; set; }
        public int WaistLengthValue { get; set; }
    }

    public class ViewCartViewModel
    {
        public decimal SubTotal { get; set; }
        public int Count { get; set; }
        public IEnumerable<Cart> Carts { get; set; }
    }

    public class CartCustomMadeViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int PrevQuantity { get; set; }

        //measurement
        [Display(Name = "Waist length")]
        public bool WaistLength { get; set; }
        [Display(Name = "Burst size")]
        public bool BurstSize { get; set; }
        [Display(Name = "Shoulder length")]
        public bool ShoulderLength { get; set; }

        public int BurstSizeValue { get; set; }
        public int ShoulderLengthValue { get; set; }
        public int WaistLengthValue { get; set; }
    }

}