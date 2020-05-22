using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Core.Model
{
    public class Category:Entity
    {
        [Required]
        [BeginWIthAlphabeth]
        [StringLength(50,MinimumLength = 2,ErrorMessage ="Category name must at least be 2 characters long and not more than 50")]
        public string Name { get; set; }
    }
}
