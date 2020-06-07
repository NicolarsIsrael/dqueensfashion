using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Core.Model
{
    public class Review:Entity
    {
        [Required]
        public string Name { get; set; }
        
        [EmailAddress]
        public string Email { get; set; }
        public int Rating { get; set; }

        [Required]
        public string Comment { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
        
    }
}
