using DQueensFashion.Core.Model;
using DQueensFashion.CustomFilters;
using DQueensFashion.Models;
using DQueensFashion.Service.Contract;
using DQueensFashion.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DQueensFashion.Controllers
{
    [Authorize(Roles=AppConstant.CustomerRole)]
    [CustomerSetGlobalVariable]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IWishListService _wishListService;
        private readonly IOrderService _orderService;
        private readonly ICategoryService _categoryService;
        private readonly IImageService _imageService;
        private readonly IReviewService _reviewService;
        private readonly ILineItemService _lineItemService;

        public CustomerController(ICustomerService customerService, IWishListService wishListService, IOrderService orderService, ICategoryService categoryService,IImageService imageService,
            IReviewService reviewService,ILineItemService lineItemService)
        {
            _customerService = customerService;
            _wishListService = wishListService;
            _orderService = orderService;
            _categoryService = categoryService;
            _imageService = imageService;
            _reviewService = reviewService;
            _lineItemService = lineItemService;
        }

        // GET: Customer
        public ActionResult Index()
        {
            return RedirectToAction(nameof(Account));
        }

        public ActionResult Dashboard()
        {
            return RedirectToAction(nameof(Account));
        }

        public ActionResult Account()
        {
            Customer customer = GetLoggedInCustomer();
            if (customer == null)
                throw new Exception();

            CustomerViewModel customerModel = new CustomerViewModel()
            {
                CustomerId = customer.Id,
                CustomerEmail = customer.Email,
                CustomerFullName = customer.Fullname,
                TotalCustomerOrders = _orderService.GetAllOrdersForCustomer(customer.Id).Count(),
                TotalCustomerWishList = _wishListService.GetAllCustomerWishList(customer.Id).Count(),
            };
            return View(customerModel);
        }

        public ActionResult Wishlist()
        {
            Customer customer = GetLoggedInCustomer();
            if (customer == null)
                throw new Exception();

            IEnumerable<ViewWishListViewModel> wishLists = _wishListService.GetAllCustomerWishList(customer.Id)
                .Select(w => new ViewWishListViewModel()
                {
                    ProductId = w.ProductId,
                    ProductImagePath = w.ProductImagePath,
                    ProductName = w.ProductName,
                    WishListId=w.Id,
                    CategoryId = w.CategoryId,
                    CategoryName = w.CategoryName,
                }).ToList();

            return View(wishLists);
        }

        public ActionResult SearchWishList(string searchString)
        {
            Customer customer = GetLoggedInCustomer();
            if (customer == null)
                throw new Exception();

            IEnumerable<ViewWishListViewModel> wishLists = _wishListService.GetAllCustomerWishList(customer.Id)
                .Select(w => new ViewWishListViewModel()
                {
                    ProductId = w.ProductId,
                    ProductImagePath = w.ProductImagePath,
                    ProductName = w.ProductName.Length > 50 ? w.ProductName.Substring(0, 47) + "..." : w.ProductName,
                    WishListId = w.Id,
                    CategoryName = w.CategoryName,
                    CategoryId = w.CategoryId,
                }).ToList();

            if (!string.IsNullOrEmpty(searchString))
                wishLists = wishLists.Where(w => w.ProductName.ToLower().Contains(searchString.ToLower()));

            return PartialView("_wishlistTable",wishLists);
        }

        public ActionResult RemoveFromWishList(int id, string searchString)
        {
            try
            {
                var customer = GetLoggedInCustomer();
                if (customer == null)
                    throw new Exception();

                WishList wishList = _wishListService.GetWishListById(id);
                if (wishList == null)
                    throw new Exception();

                if (wishList.CustomerId != customer.Id)
                    throw new Exception();

                _wishListService.DeleteWishList(wishList);

                IEnumerable<ViewWishListViewModel> wishLists = _wishListService.GetAllCustomerWishList(customer.Id)
                    .Select(w => new ViewWishListViewModel()
                    {
                        ProductId = w.ProductId,
                        ProductImagePath = w.ProductImagePath,
                        ProductName = w.ProductName,
                        WishListId = w.Id,
                    }).ToList();

                if (!string.IsNullOrEmpty(searchString))
                    wishLists = wishLists.Where(w => w.ProductName.ToLower().Contains(searchString.ToLower()));

                return PartialView("_wishlistTable", wishLists);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public ActionResult Orders()
        {
            Customer customer = GetLoggedInCustomer();
            if (customer == null)
                throw new Exception(); ;

            IEnumerable<ViewOrderViewModel> orderModel = _orderService.GetAllOrdersForCustomer(customer.Id)
                .Select(order => new ViewOrderViewModel()
                {
                    OrderId = order.Id,
                    CustomerId = order.CustomerId,
                    TotalAmount = order.TotalAmount,
                    TotalQuantity = order.TotalQuantity,
                    LineItems = order.LineItems
                        .Select(lineItem => new ViewLineItem()
                        {
                            ProductName = lineItem.Product.Name,
                            Quantity = lineItem.Quantity,
                            TotalAmount = lineItem.TotalAmount,
                        }),
                    OrderStatus = order.OrderStatus.ToString(),
                    DateCreated = order.DateCreated,
                    DateCreatedString = order.DateCreated.ToString("dd/MMM/yyyy : hh-mm-ss"),
                }).OrderBy(order => order.DateCreated).ToList();

            return View(orderModel);
        }

        public ActionResult SearchOrders(string searchString)
        {
            Customer customer = GetLoggedInCustomer();
            if (customer == null)
                throw new Exception(); ;

            IEnumerable<ViewOrderViewModel> orderModel = _orderService.GetAllOrdersForCustomer(customer.Id)
                .Select(order => new ViewOrderViewModel()
                {
                    OrderId = order.Id,
                    CustomerId = order.CustomerId,
                    TotalAmount = order.TotalAmount,
                    TotalQuantity = order.TotalQuantity,
                    LineItems = order.LineItems
                        .Select(lineItem => new ViewLineItem()
                        {
                            ProductName = lineItem.Product.Name,
                            Quantity = lineItem.Quantity,
                            TotalAmount = lineItem.TotalAmount,
                        }),
                    OrderStatus = order.OrderStatus.ToString(),
                    DateCreated = order.DateCreated,
                    DateCreatedString = order.DateCreated.ToString("dd/MMM/yyyy : hh-mm-ss"),
                    LineItemConcatenatedString = string.Join(",", order.LineItems.Select(x => x.Product.Name)),
                }).OrderBy(order => order.DateCreated).ToList();

            if (!string.IsNullOrEmpty(searchString))
                orderModel = orderModel.Where(order => order.LineItemConcatenatedString.ToLower().Contains(searchString.ToLower())
                || order.OrderStatus.ToString().ToLower().Contains(searchString.ToLower())
                || (string.Compare(order.OrderId.ToString(), searchString, true) == 0)
                ).ToList();


            return PartialView("_ordersTable", orderModel);
        }

        public ActionResult OrderDetails(int id = 0)
        {
            Order order = _orderService.GetOrderById(id);
            if (order == null)
                return HttpNotFound();

            Customer customer = GetLoggedInCustomer();
            if (customer == null)
                throw new Exception();

            if (order.CustomerId != customer.Id)
                throw new Exception();

            var allImages = _imageService.GetAllImageFiles();

            ViewOrderViewModel orderModel = new ViewOrderViewModel()
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                TotalAmount = order.TotalAmount,
                TotalQuantity = order.TotalQuantity,
                LineItems = order.LineItems
                        .Select(lineItem => new ViewLineItem()
                        {
                            LineItemId = lineItem.Id,
                            ProductName = lineItem.Product.Name,
                            ProductId = lineItem.Product.Id,
                            Quantity = lineItem.Quantity,
                            UnitPrice = lineItem.UnitPrice,
                            TotalAmount = lineItem.TotalAmount,
                            ProductImage = allImages.Where(image => image.ProductId == lineItem.Product.Id).Count() < 1 ?
                                AppConstant.DefaultProductImage :
                                allImages.Where(image => image.ProductId == lineItem.Product.Id).FirstOrDefault().ImagePath,
                            Description = lineItem.Description,
                            CanReview = _reviewService.CanReview(lineItem.Id),
                        }),
                OrderStatus = order.OrderStatus.ToString(),
                DateCreated = order.DateCreated,
                DateCreatedString = order.DateCreated.ToString("dd/MMM/yyyy - hh:mm:ss"),
                LineItemConcatenatedString = string.Join(",", order.LineItems.Select(x => x.Product.Name)),

            };


            return View(orderModel);
        }

        [Authorize(Roles = AppConstant.CustomerRole)]
        public ActionResult AddReview(int id = 0)
        {
            if (!_reviewService.CanReview(id))
                return HttpNotFound();

            LineItem lineItem = _lineItemService.GetLineItemById(id);
            if (lineItem == null)
                return HttpNotFound();

            Product product = lineItem.Product;
            if (product == null)
                throw new Exception();

            Customer customer = GetLoggedInCustomer();
            if (customer == null)
                throw new Exception();

            double averageRating = _reviewService.GetAverageRating(product.Id);
            var allProductImages = _imageService.GetImageFilesForProduct(product.Id);

            AddReviewViewModel reviewModel = new AddReviewViewModel()
            {
                LineItemId = lineItem.Id,
                ProductId = product.Id,
                ProductName = product.Name.Length > 20 ? product.Name.Substring(0, 20) + "..." : product.Name,
                ProductImage = allProductImages.Count() < 1 ?
                        AppConstant.DefaultProductImage :
                        _imageService.GetMainImageForProduct(product.Id).ImagePath,
                ProductCategory = product.Category.Name,
                Name = customer.Fullname,
            };

            return View(reviewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddReview(AddReviewViewModel reviewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "One or more validation errors");
                return View(reviewModel);
            }

            if (!_reviewService.CanReview(reviewModel.LineItemId))
                return HttpNotFound();

            LineItem lineItem = _lineItemService.GetLineItemById(reviewModel.LineItemId);
            if (lineItem == null)
                throw new Exception();

            Product product = lineItem.Product;
            if (product == null)
                throw new Exception();

            Customer customer = GetLoggedInCustomer();
            if (customer == null)
                throw new Exception();

            Review review = new Review()
            {
                Name = reviewModel.Name,
                Email = customer.Email,
                Comment = reviewModel.Comment,
                Rating = reviewModel.Rating,
                Product = product,
                CustomerId = customer.Id,
                LineItemId = reviewModel.LineItemId,
            };

            _reviewService.AddReview(review);
            return RedirectToAction(nameof(OrderDetails), new { id = lineItem.Order.Id });
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

        #region private function
        private string GetLoggedInUserId()
        {
            return User.Identity.GetUserId();
        }

        private Customer GetLoggedInCustomer()
        {
            var userId = GetLoggedInUserId();
            return _customerService.GedCustomerByUserId(userId);
        }

        #endregion

    }
}