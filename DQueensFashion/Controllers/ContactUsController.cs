﻿using DQueensFashion.Models;
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
    }
}