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
            IEnumerable<ViewProductsViewModel> products = _productService.GetAllProducts()
                .Select(p => new ViewProductsViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description.Length > 35 ? p.Description.Substring(0, 35) + "..." : p.Description,
                    Image1 = p.ImagePath1,
                    Quantity = p.Quantity.ToString(),
                    Price=p.Price.ToString(),

                }).ToList();
            HomeIndexViewModel homeIndex = new HomeIndexViewModel()
            {
                Products = products,
            };

            ViewBag.CartCount = Session["cart"]==null?0: ((List<AddCartViewModel>)Session["cart"]).Sum(c=>c.Quantity);
            return View(homeIndex);
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