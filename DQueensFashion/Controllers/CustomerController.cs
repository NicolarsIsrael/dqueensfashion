using DQueensFashion.Core.Model;
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
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IWishListService _wishListService;
        private readonly IOrderService _orderService;

        public CustomerController(ICustomerService customerService, IWishListService wishListService, IOrderService orderService)
        {
            _customerService = customerService;
            _wishListService = wishListService;
            _orderService = orderService;
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

        public ActionResult ViewOrders()
        {
            Customer customer = GetLoggedInCustomer();
            if (customer == null)
                throw new Exception(); ;

            IEnumerable<ViewOrderViewModel> orderModel = _orderService.GetAllOrdersForCustomer(customer.Id)
                .Select(order => new ViewOrderViewModel()
                {
                    OrderId = order.Id,
                    CustomerId = order.Customer.Id,
                    TotalAmount = order.TotalAmount,
                    TotalQuantity = order.TotalQuantity,
                    LineItems = order.LineItems
                        .Select(lineItem => new ViewLineItem()
                        {
                            Product = lineItem.Product.Name,
                            Quantity = lineItem.Quantity,
                            TotalAmount = lineItem.TotalAmount,
                        }),
                    OrderStatus = order.OrderStatus.ToString(),
                    DateCreated = order.DateCreated,
                    DateCreatedString = order.DateCreated.ToString("dd/MMM/yyyy : hh-mm-ss"),
                }).OrderBy(order => order.DateCreated).ToList();

            return View(orderModel);
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