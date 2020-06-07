using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Core.Model
{
    public class Review:Entity
    {
        public int Rating { get; set; }
        public string Summary { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
        
    }
}
