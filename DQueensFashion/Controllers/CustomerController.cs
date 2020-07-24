﻿using DQueensFashion.Core.Model;
using DQueensFashion.CustomFilters;
using DQueensFashion.Models;
using DQueensFashion.Service.Contract;
using DQueensFashion.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IProductService _productService;
        private readonly IMailingListService _mailingListService;
        private ApplicationUserManager _userManager;
        private GeneralService generalService;

        public CustomerController(ICustomerService customerService, IWishListService wishListService, IOrderService orderService, ICategoryService categoryService,IImageService imageService,IReviewService reviewService,ILineItemService lineItemService,IProductService productService,
            IMailingListService mailingListService, ApplicationUserManager userManager)
        {
            _customerService = customerService;
            _wishListService = wishListService;
            _orderService = orderService;
            _categoryService = categoryService;
            _imageService = imageService;
            _reviewService = reviewService;
            _lineItemService = lineItemService;
            _productService = productService;
            _mailingListService = mailingListService;
            _userManager = userManager;
            generalService = new GeneralService();
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
                TotalCustomerOrders = _orderService.GetAllOrdersForCustomer(customer.Id).Count(),
                TotalCustomerWishList = _wishListService.GetAllCustomerWishList(customer.Id).Count(),
                TotalCustomerPendingReviews = _reviewService.GetPendingReviews(customer.Id).Count(),
                IsSubscribed = _mailingListService.CheckIfSubscribed(customer.Id),
            };
            return View(customerModel);
        }

        public ActionResult Wishlist(string query ="")
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
                    ProductPrice = w.ProductPrice,
                    GeneratedUrl = generalService.GenerateItemNameAsParam(w.ProductId,w.ProductName),
                }).ToList();

            foreach(var wishlist in wishLists)
            {
                if (_productService.GetProductById(wishlist.ProductId) == null)
                {
                    wishLists.ToList().Remove(wishlist);
                    _wishListService.DeleteWishList(_wishListService.GetWishListById(wishlist.WishListId));
                }
                else
                {
                    wishlist.IsOutOfStock = _productService.GetProductById(wishlist.ProductId).Quantity < 1 ? true : false;
                }
            }

            if (!string.IsNullOrEmpty(query))
                wishLists = wishLists.Where(w => w.ProductName.ToLower().Contains(query.ToLower()) 
                || w.CategoryName.ToLower().Contains(query.ToLower()));

            ViewBag.Query = query;
            return View(wishLists);
        }

        public ActionResult RemoveFromWishList(int id, string query)
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

                return Content("");
            }
            catch (Exception)
            {

                throw;
            }
        }


        public ActionResult Orders(string query="")
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
                    DateCreated = order.DateCreatedUtc,
                    DateCreatedString = generalService.GetDateInString(order.DateCreated, false, false),
                    LineItemConcatenatedString = string.Join(",", order.LineItems.Select(x => x.Product.Name)),
                }).OrderByDescending(order => order.DateCreated).ToList();

            if (!string.IsNullOrEmpty(query))
                orderModel = orderModel.Where(order => order.LineItemConcatenatedString.ToLower().Contains(query.ToLower())
                || order.OrderStatus.ToString().ToLower().Contains(query.ToLower())
                || (string.Compare(order.OrderId.ToString(), query, true) == 0)
                ).ToList();

            ViewBag.Query = query;
            return View(orderModel);
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
                return HttpNotFound();

            var allImages = _imageService.GetAllImageMainFiles();

            ViewOrderViewModel orderModel = new ViewOrderViewModel()
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                SubTotal = order.SubTotal,
                ShippingPrice = order.ShippingPrice,
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
                            Description = string.IsNullOrEmpty(lineItem.Description) ? "" : lineItem.Description.Replace("\r\n", " , "),
                            CanReview = _reviewService.CanReview(lineItem.Id),
                            IsOutfit = lineItem.Product.CategoryId == AppConstant.OutfitsId ? true : false,
                        }),
                OrderStatus = order.OrderStatus.ToString(),
                DateCreated = order.DateCreatedUtc,
                DateCreatedString = generalService.GetDateInString(order.DateCreated, true, false),
                LineItemConcatenatedString = string.Join(",", order.LineItems.Select(x => x.Product.Name)),
            };


            return View(orderModel);
        }


        public ActionResult AddReview(int id = 0)
        {
            Customer customer = GetLoggedInCustomer();
            if (customer == null)
                throw new Exception();

            if (!_reviewService.CanReview(id))
                return HttpNotFound();

            LineItem lineItem = _lineItemService.GetLineItemById(id);
            if (lineItem == null)
                return HttpNotFound();

            Product product = lineItem.Product;
            if (product == null)
                throw new Exception();

            Order order = _orderService.GetOrderById(lineItem.Order.Id);
            if (customer.Id != order.CustomerId)
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
                Name = lineItem.Order.FirstName + " " + lineItem.Order.LastName,
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

            Order order = _orderService.GetOrderById(lineItem.Order.Id);
            if (customer.Id != order.CustomerId)
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

        public ActionResult PendingReviews()
        {
            Customer customer = GetLoggedInCustomer();
            if (customer == null)
                throw new Exception();

            var allImages = _imageService.GetAllImageFiles();

            IEnumerable<ViewLineItem> lineItems = _reviewService.GetPendingReviews(customer.Id)
                .Select(l => new ViewLineItem()
                {
                    LineItemId = l.Id,
                    ProductName = l.Product.Name,
                    ProductId = l.Product.Id,
                    Quantity = l.Quantity,
                    UnitPrice = l.UnitPrice,
                    TotalAmount = l.TotalAmount,
                    ProductImage = allImages.Where(image => image.ProductId == l.Product.Id).Count() < 1 ?
                                AppConstant.DefaultProductImage :
                                allImages.Where(image => image.ProductId == l.Product.Id).FirstOrDefault().ImagePath,
                    Description = l.Description,
                });

            return View(lineItems);
        }

        public ActionResult Subscription()
        {
            Customer customer = GetLoggedInCustomer();
            if (customer == null)
                throw new Exception();
            ViewBag.Email = customer.Email;
            ViewBag.Subscription = _mailingListService.CheckIfSubscribed(customer.Id);
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel passwordModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "One or more validation errors");
                return View(passwordModel);
            }

            var userId = GetLoggedInUserId();
            var result = await _userManager.ChangePasswordAsync(userId, passwordModel.OldPassword, passwordModel.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Account));
            }
            else
            {
                throw new Exception();
            }
        }

        public int GetPendingReviewsCount()
        {
            Customer customer = GetLoggedInCustomer();
            if (customer == null)
                throw new Exception();

            return _reviewService.GetPendingReviews(customer.Id).Count();
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