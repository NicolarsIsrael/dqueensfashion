using DQueensFashion.Core.Model;
using DQueensFashion.Models;
using DQueensFashion.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DQueensFashion.Controllers
{
    public class ContactUsController : Controller
    {

        private readonly IMessageService _messageService;
        public ContactUsController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        // GET: ContactUs
        public ActionResult Index()
        {
            ContactUsViewModel contactUsModel = new ContactUsViewModel();

            return View(contactUsModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ContactUsViewModel contactUsModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "One or more validation errors");
                return View();
            }

            var message = new DQueensFashion.Core.Model.Message()
            {
                Fullname = contactUsModel.Fullname,
                Email = contactUsModel.Email,
                Phone = contactUsModel.Phone,
                Subject = contactUsModel.Subject,
                MessageSummary = contactUsModel.Message,
            };

            _messageService.AddMessage(message);

            return RedirectToAction(nameof(Index));
        }
    }
}