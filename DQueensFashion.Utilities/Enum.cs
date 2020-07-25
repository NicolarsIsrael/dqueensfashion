using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Utilities
{
    public enum OrderStatus
    {
        Processing,
        [Display(Name ="In transit")]
        InTransit,
        Delivered,
        Returned,
        Completed
    }

    public enum QuantityVariation
    {
        Yards,
        Pieces,
    }

}
