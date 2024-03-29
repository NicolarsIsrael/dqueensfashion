﻿using DQueensFashion.Core.Model;
using DQueensFashion.Models;
using DQueensFashion.Service.Contract;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DQueensFashion.Controllers
{
    public class WishlistController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IWishListService _wishListService;
        private readonly IProductService _productService;

        public WishlistController(ICustomerService customerService, IWishListService wishListService, IProductService productService)
        {
            _customerService = customerService;
            _wishListService = wishListService;
            _productService = productService;
        }
        // GET: Wishlist
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult AddToWishList(int id)
        {
            try
            {
                var customer = GetLoggedInCustomer();
                if (customer == null)
                    return Json("login", JsonRequestBehavior.AllowGet);

                Product product = _productService.GetProductById(id);
                if (product == null)
                    throw new Exception(); //alert product doesnt exist

                WishList wishList = new WishList()
                {
                    CustomerId = customer.Id,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductImagePath = product.ImagePath1,
                };
                _wishListService.AddWishList(wishList);
                return Json("success", JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                throw;
            }

        }

        public ActionResult RemoveFromWishList(int id)
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
                        WishListId=w.Id,
                    }).ToList();


                return PartialView("_wishListTable", wishLists);
            }
            catch (Exception)
            {

                throw;
            }
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