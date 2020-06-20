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

        public CustomerController(ICustomerService customerService, IWishListService wishListService, IOrderService orderService, ICategoryService categoryService)
        {
            _customerService = customerService;
            _wishListService = wishListService;
            _orderService = orderService;
            _categoryService = categoryService;
        }

        // GET: Customer
        public ActionResult Index()
        {
            return RedirectToAction(nameof(Dashboard));
        }

        public ActionResult Dashboard()
        {
            return View();
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
                    ProductName = w.ProductName,
                    WishListId = w.Id,
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