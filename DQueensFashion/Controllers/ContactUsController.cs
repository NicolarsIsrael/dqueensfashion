using DQueensFashion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DQueensFashion.Controllers
{
    public class ContactUsController : Controller
    {
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



            return View();
        }
    }
}