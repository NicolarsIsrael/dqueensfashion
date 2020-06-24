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
        public decimal Discount { get; set; }
        public decimal InitialPrice { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string Description { get; set; }

        //Ready made sizes
        public int ExtraSmallQuantity { get; set; }
        public int SmallQuantiy { get; set; }
        public int MediumQuantiy { get; set; }
        public int LargeQuantity { get; set; }
        public int ExtraLargeQuantity { get; set; }
        public string ReadyMadeSize { get; set; }


        //measurement
        public bool Shoulder { get; set; }
        public double ShoulderValue { get; set; }

        [Display(Name = "Arm hole")]
        public bool ArmHole { get; set; }
        public double ArmHoleValue { get; set; }

        public bool Burst { get; set; }
        public double BurstValue { get; set; }

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
    }

    //public class CartViewModel
    //{
    //    public int ProductId { get; set; }
    //    public string ProductName { get; set; }
    //    public int Quantity { get; set; }
    //    public int PrevQuantity { get; set; }

    //    //Ready made sizes
    //    public int ExtraSmallQuantity { get; set; }
    //    public int SmallQuantiy { get; set; }
    //    public int MediumQuantiy { get; set; }
    //    public int LargeQuantity { get; set; }
    //    public int ExtraLargeQuantity { get; set; }
    //    public string ReadyMadeSize { get; set; }

    //    //measurement

    //    //measurement
    //    public bool? Shoulder { get; set; }
    //    public string ShoulderValue { get; set; }

    //    [Display(Name = "Arm hole")]
    //    public bool? ArmHole { get; set; }
    //    public string ArmHoleValue { get; set; }

    //    public bool? Burst { get; set; }
    //    public string BurstValue { get; set; }

    //    public bool? Waist { get; set; }
    //    public string WaistValue { get; set; }

    //    public bool? Hips { get; set; }
    //    public string HipsValue { get; set; }

    //    public bool? Thigh { get; set; }
    //    public string ThighValue { get; set; }

    //    [Display(Name = "Full body length")]
    //    public bool? FullBodyLength { get; set; }
    //    public string FullBodyLengthValue { get; set; }

    //    [Display(Name = "Knee garment length")]
    //    public bool? KneeGarmentLength { get; set; }
    //    public string KneeGarmentLengthValue { get; set; }

    //    [Display(Name = "Top length")]
    //    public bool? TopLength { get; set; }
    //    public string TopLengthValue { get; set; }

    //    [Display(Name = "Trouser length")]
    //    public bool? TrousersLength { get; set; }
    //    public string TrousersLengthValue { get; set; }

    //    [Display(Name = "Round neck")]
    //    public bool? RoundNeck { get; set; }
    //    public string RoundNeckValue { get; set; }

    //    [Display(Name = "Nip nip")]
    //    public bool? NipNip { get; set; }
    //    public string NipNipValue { get; set; }

    //    [Display(Name = "Sleeve length")]
    //    public bool? SleeveLength { get; set; }
    //    public string SleeveLengthValue { get; set; }

    //}

}