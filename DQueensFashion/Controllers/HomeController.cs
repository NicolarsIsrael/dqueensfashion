using DQueensFashion.Core.Model;
using DQueensFashion.CustomFilters;
using DQueensFashion.Models;
using DQueensFashion.Service.Contract;
using DQueensFashion.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DQueensFashion.Controllers
{
    [HomeSetGlobalVariable]
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ICustomerService _customerService;
        private readonly IWishListService _wishListService;
        private readonly ILineItemService _lineItemService;
        private readonly IOrderService _orderService;
        private readonly IReviewService _reviewService;

        public HomeController(IProductService productService, ICategoryService categoryService,ICustomerService customerService, IWishListService wishListService,
            ILineItemService lineItemService, IOrderService orderService, IReviewService reviewService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _customerService = customerService;
            _wishListService = wishListService;
            _lineItemService = lineItemService;
            _orderService = orderService;
            _reviewService = reviewService;
        }
        public ActionResult Index()
        {

            IEnumerable<ViewProductsViewModel> products = _productService.GetAllProducts()
                .Select(p => new ViewProductsViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description.Length > 35 ? p.Description.Substring(0, 35) + "..." : p.Description,
                    MainImage = string.IsNullOrEmpty(p.ImagePath1) ? AppConstant.DefaultProductImage : p.ImagePath1,
                    Quantity = p.Quantity.ToString(),
                    Price=p.Price.ToString(),
                    Category = p.Category.Name,
                    CategoryId = p.Category.Id,
                    Rating = new RatingViewModel()
                    {
                        AverageRating = p.AverageRating.ToString("0.0"),
                        TotalReviewCount = _reviewService.GetReviewCountForProduct(p.Id).ToString(),
                        IsDouble = (p.AverageRating % 1) == 0 ? false : true,
                        FloorAverageRating = (int)Math.Floor(p.AverageRating)
                    },
                    
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


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            ViewBag.ReviewCount = _reviewService.GetAllReviewCount();
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