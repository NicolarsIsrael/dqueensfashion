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

        public CustomerController(ICustomerService customerService, IWishListService wishListService)
        {
            _customerService = customerService;
            _wishListService = wishListService;
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