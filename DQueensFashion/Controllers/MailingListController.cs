using DQueensFashion.Core.Model;
using DQueensFashion.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DQueensFashion.Controllers
{
    public class MailingListController : Controller
    {
        private readonly IMailingListService _mailingListService;
        public MailingListController(IMailingListService mailingListService)
        {
            _mailingListService = mailingListService;
        }

        public ActionResult Subscribe(string email)
        {
            if (!IsValidEmail(email))
                return Content("Invalid");
            MailingList mailingList = new MailingList()
            {
                EmailAddress = email
            };

            _mailingListService.AddToMailingList(mailingList);
            return Content("Success");
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}