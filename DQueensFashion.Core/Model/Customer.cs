using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Core.Model
{
    public class Customer:Entity
    {
        public string Email { get; set; }
        public string UserId { get; set; }
        public bool? AvailableSubcriptionDiscount { get; set; }
        public bool? UsedSubscriptionDiscount { get; set; }
    }
}
