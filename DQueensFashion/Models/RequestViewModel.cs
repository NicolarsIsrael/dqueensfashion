using DQueensFashion.Core.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DQueensFashion.Models
{
    public class RequestViewModel
    {
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public string MainImage { get; set; }
        public int Quantity { get; set; }
        public string CustomerEmail { get; set; }
    }

    public class ViewRequestsViewModel
    {
        public string MainImage { get; set; }
        public Product Product { get; set; }
        public string ProductName { get; set; }
        public IEnumerable<RequestViewModel> UsersRequests { get; set; }
    }

    public class RequestsReplyViewModel
    {
        public List<EmailsViewModel> EmailList { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string Message { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public string ProductLink { get; set; }
        public string MainImage { get; set; }
    }

}