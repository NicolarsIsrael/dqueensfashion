using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Core.Model
{
    public class Product:Entity
    {
        public Product()
        {
            Images = new List<ImageFile>();
            Reviews = new List<Review>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal SubTotal { get; set; }
        public int Quantity { get; set; }
        public string Tags { get; set; }
        public double AverageRating { get; set; }
        public IEnumerable<ImageFile> Images { get; set; }
        public int SizeChartImageId { get; set; }
        public int DeliveryDaysDuration { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public IEnumerable<Review> Reviews { get; set; }

        //Ready made sizes
        public int ExtraSmallQuantity { get; set; }
        public int SmallQuantiy { get; set; }
        public int MediumQuantiy { get; set; }
        public int LargeQuantity { get; set; }
        public int ExtraLargeQuantity { get; set; }


        //measurement
        public bool? Shoulder { get; set; }
        public bool? ArmHole { get; set; }
        public bool? Burst { get; set; }
        public bool? Waist { get; set; }
        public bool? Hips { get; set; }
        public bool? Thigh { get; set; }
        public bool? FullBodyLength { get; set; }
        public bool? KneeGarmentLength { get; set; }
        public bool? TopLength { get; set; }
        public bool? TrousersLength { get; set; }
        public bool? RoundAnkle { get; set; }
        public bool? NipNip { get; set; }
        public bool? SleeveLength { get; set; }
    }
}
