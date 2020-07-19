using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Core.Model
{
    public class Request: Entity
    {
        public string CustomerEmail { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
    }
}
