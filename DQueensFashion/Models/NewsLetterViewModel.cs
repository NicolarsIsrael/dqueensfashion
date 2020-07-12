using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DQueensFashion.Models
{
    public class CreateNewsLetterViewModel
    {
        public List<SubscribedEmailsViewModel> EmailList { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string Message { get; set; }

    }

    public class SubscribedEmailsViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool IsSelected { get; set; }
    }
}