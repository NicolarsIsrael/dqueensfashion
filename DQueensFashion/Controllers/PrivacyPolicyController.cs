﻿using DQueensFashion.CustomFilters;
using DQueensFashion.Models;
using DQueensFashion.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DQueensFashion.Controllers
{
   [PrivacyPolicySetGlobalVariable]
    public class PrivacyPolicyController : Controller
    {
        private readonly ICategoryService _categoryService;
        public PrivacyPolicyController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: PrivacyPolicy
        public ActionResult Index()
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