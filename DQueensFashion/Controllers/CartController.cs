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
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        public CartController(IProductService productService)
        {
            _productService = productService;
        }
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddToCart(int id)
        {
            Product product = _productService.GetProductById(id);
            if (product == null)
                throw new Exception();

            if (Session["cart"] == null)
            {
                List<AddCartViewModel> cart = new List<AddCartViewModel>();
                cart.Add(new AddCartViewModel { Product = product, Quantity = 1 });
                Session["cart"] = cart;
            }
            else
            {
                List<AddCartViewModel> cart = (List<AddCartViewModel>)Session["cart"];
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new AddCartViewModel { Product = product, Quantity = 1 });
                }
                Session["cart"] = cart;
            }
            ViewCartViewModel viewCart = new ViewCartViewModel()
            {
                Count = Session["cart"] == null ? 0 : ((List<AddCartViewModel>)Session["cart"]).Sum(c => c.Quantity),
            };

            return PartialView("_cartCount",viewCart);
        }

        public ActionResult GetCart()
        {
            ViewCartViewModel viewCart = new ViewCartViewModel()
            {
                Count = Session["cart"] == null ? 0 : ((List<AddCartViewModel>)Session["cart"]).Sum(c => c.Quantity),
            };

            return PartialView("_cartCount", viewCart);
        }

        private int isExist(int id)
        {
            List<AddCartViewModel> cart = (List<AddCartViewModel>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Product.Id.Equals(id))
                    return i;
            return -1;
        }
    }
}