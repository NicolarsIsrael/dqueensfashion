﻿using DQueensFashion.Core.Model;
using DQueensFashion.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DQueensFashion.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ICustomerService _customerService;
        public HomeController(IProductService productService, ICategoryService categoryService,ICustomerService customerService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _customerService = customerService;
        }
        public ActionResult Index()
        {

            ViewBag.CategoryCount = _categoryService.GetAllCategoriesCount();
            ViewBag.ProductsCount = _productService.GetAllProductsCount();
            ViewBag.CustomerCount = _customerService.GetAllCustomerCount();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}