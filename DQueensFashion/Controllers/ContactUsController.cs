using DQueensFashion.Core.Model;
using DQueensFashion.Models;
using DQueensFashion.Service.Contract;
using DQueensFashion.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult> Index(ContactUsViewModel contactUsModel)
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

            //send mail
            try
            {
                string messageSumarry = $"You receieved a message from {contactUsModel.Fullname} with Email address: {contactUsModel.Email}" +
                     $" , Phone: {contactUsModel.Phone} <br />"  +
                     $"Message: <br /> {contactUsModel.Message}";

                var credentials = AppConstant.MAIL_CREDENTIALS;
                MailService mailService = new MailService();
                await mailService.SendMail(AppConstant.HDQ_EMAIL_ACCOUNT, contactUsModel.Subject, messageSumarry, credentials);
            }
            catch (Exception) {}


            return RedirectToAction(nameof(ThankYou));
        }

        public ActionResult ThankYou()
        {
            return View();
        }
    }
}