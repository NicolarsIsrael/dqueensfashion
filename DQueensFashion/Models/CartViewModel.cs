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
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string MainImage { get; set; }
        public int Quantity { get; set; }
        public int MaxQuantity { get; set; }
        public decimal Discount { get; set; }
        public decimal InitialPrice { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string Description { get; set; }

        //measurement
        public bool Shoulder { get; set; }
        public double ShoulderValue { get; set; }

        [Display(Name = "Arm hole")]
        public bool ArmHole { get; set; }
        public double ArmHoleValue { get; set; }

        public bool Bust { get; set; }
        public double BustValue { get; set; }

        public bool Waist { get; set; }
        public double WaistValue { get; set; }

        public bool Hips { get; set; }
        public double HipsValue { get; set; }

        public bool Thigh { get; set; }
        public double ThighValue { get; set; }

        [Display(Name = "Full body length")]
        public bool FullBodyLength { get; set; }
        public double FullBodyLengthValue { get; set; }

        [Display(Name = "Knee garment length")]
        public bool KneeGarmentLength { get; set; }
        public double KneeGarmentLengthValue { get; set; }

        [Display(Name = "Top length")]
        public bool TopLength { get; set; }
        public double TopLengthValue { get; set; }

        [Display(Name = "Trouser length")]
        public bool TrousersLength { get; set; }
        public double TrousersLengthValue { get; set; }

        [Display(Name = "Round ankle")]
        public bool RoundAnkle { get; set; }
        public double RoundAnkleValue { get; set; }

        [Display(Name = "Nip nip")]
        public bool NipNip { get; set; }
        public double NipNipValue { get; set; }

        [Display(Name = "Sleeve length")]
        public bool SleeveLength { get; set; }
        public double SleeveLengthValue { get; set; }
    }

    public class ViewCartViewModel
    {
        public decimal SubTotal { get; set; }
        public int Count { get; set; }
        public IEnumerable<Cart> Carts { get; set; }


        //shipping details
        [Required(ErrorMessage ="First name is requied")]
        [Display(Name ="First name")]
        public string FirstName { get; set; }

        [Display(Name ="Last name")]
        public string LastName { get; set; }

        [Display(Name = "Phone number")]
        [Required(ErrorMessage ="Phone number is required")]
        [Phone]
        public string Phone { get; set; }

        [Required(ErrorMessage ="Address is required")]
        public string Address { get; set; }
    }

}