using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Core.Model
{
    public class Customer:Entity
    {
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
    }
}
