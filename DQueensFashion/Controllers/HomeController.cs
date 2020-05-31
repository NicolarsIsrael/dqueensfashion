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
        private readonly IWishListService _wishListService;
        private readonly ILineItemService _lineItemService;
        private readonly IOrderService _orderService;

        public HomeController(IProductService productService, ICategoryService categoryService,ICustomerService customerService, IWishListService wishListService,
            ILineItemService lineItemService, IOrderService orderService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _customerService = customerService;
            _wishListService = wishListService;
            _lineItemService = lineItemService;
            _orderService = orderService;
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
                    Category = p.Category.Name,
                    CategoryId = p.Category.Id,
                }).ToList();

            IEnumerable<CategoryNameAndId> categories = _categoryService.GetAllCategories()
                .Select(category => new CategoryNameAndId()
                {
                    Id = category.Id,
                    Name = category.Name.ToUpper(),
                }).OrderBy(c=>c.Name).ToList();

            HomeIndexViewModel homeIndex = new HomeIndexViewModel()
            {
                Products = products,
                Categories = categories,
            };

            ViewBag.LineItemCount = _lineItemService.GetLineItemsCount();
            ViewBag.OrderCount = _orderService.GetOrderCount();
            return View(homeIndex);
        }

        public ActionResult GetCategories()
        {

            ViewCartViewModel viewCart = new ViewCartViewModel()
            {
                Count = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.Quantity),
                Carts = Session["cart"] == null ? new List<Cart>() : (List<Cart>)Session["cart"],
            };

            IEnumerable<ViewCategoryViewModel> categories = _categoryService.GetAllCategories()
                .Select(c => new ViewCategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                }).OrderBy(c=>c.Name);

            return PartialView("_navbarCategories", categories);
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