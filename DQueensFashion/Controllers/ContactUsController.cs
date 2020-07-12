using DQueensFashion.Core.Model;
using DQueensFashion.CustomFilters;
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
    [ContactUsSetGlobalVariable]
    public class ContactUsController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly ICategoryService _categoryService;
        
        public ContactUsController(IMessageService messageService, ICategoryService categoryService)
        {
            _messageService = messageService;
            _categoryService = categoryService;
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



        public int GetCartNumber()
        {
            if (Session["cart"] != null)
                return ((List<Cart>)Session["cart"]).Sum(c => c.Quantity);
            else
                return 0;
        }

        public IEnumerable<CategoryNameAndId> GetCategories()
        {
            IEnumerable<CategoryNameAndId> categories = _categoryService.GetAllCategories()
                .Select(c => new CategoryNameAndId()
                {
                    Id = c.Id,
                    Name = c.Name,
                }).OrderBy(c => c.Name);

            return categories;
        }

    }
}