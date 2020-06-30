using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Core.Model
{
    public class MailingList:Entity
    {
        [EmailAddress]
        [Display(Name ="Email address")]
        public string EmailAddress { get; set; }
    }
}
