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
                List<Cart> cart = new List<Cart>();
                cart.Add(new Cart { Product = product, Quantity = 1,Price = product.Price });
                Session["cart"] = cart;
            }
            else
            {
                List<Cart> cart = (List<Cart>)Session["cart"];
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Quantity++;
                    cart[index].Price = cart[index].Product.Price * cart[index].Quantity;
                }
                else
                {
                    cart.Add(new Cart { Product = product, Quantity = 1, Price = product.Price });
                }
                Session["cart"] = cart;
            }

            ViewCartViewModel viewCart = new ViewCartViewModel()
            {
                Count = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.Quantity),
                Carts = Session["cart"] == null ? new List<Cart>() : (List<Cart>)Session["cart"],
            };

            return PartialView("_navbarCart", viewCart);
        }

        public ActionResult GetCart()
        {
            ViewCartViewModel viewCart = new ViewCartViewModel()
            {
                Count = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.Quantity),
                Carts = Session["cart"] == null ? new List<Cart>() : (List<Cart>)Session["cart"],
            };

            return PartialView("_navbarCart", viewCart);
        }

        public ActionResult RemoveFromCart(int id)
        {
            List<Cart> cart = (List<Cart>)Session["cart"];
            int index = isExist(id);
            cart.RemoveAt(index);
            Session["cart"] = cart;

            ViewCartViewModel viewCart = new ViewCartViewModel()
            {
                Count = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.Quantity),
                Carts = Session["cart"] == null ? new List<Cart>() : (List<Cart>)Session["cart"],
            };

            return PartialView("_navbarCart", viewCart);
        }


        public ActionResult ViewCart()
        {
            List<Cart> cart = (List<Cart>)Session["cart"];
            ViewCartViewModel viewCart = new ViewCartViewModel()
            {
                Count = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.Quantity),
                Carts = Session["cart"] == null ? new List<Cart>() : (List<Cart>)Session["cart"],
            };
            return View(viewCart);
        }

        public ActionResult IncreaseQuantity(int id)
        {
            Product product = _productService.GetProductById(id);
            if (product == null)
                throw new Exception();

            List<Cart> cart = (List<Cart>)Session["cart"];
            int index = isExist(id);
            if (index != -1)
            {
                cart[index].Quantity++;
                cart[index].Price = cart[index].Product.Price * cart[index].Quantity;
            }
            Session["cart"] = cart;

            ViewCartViewModel viewCart = new ViewCartViewModel()
            {
                Count = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.Quantity),
                Carts = Session["cart"] == null ? new List<Cart>() : (List<Cart>)Session["cart"],
            };

            return PartialView("_increaseQuantity",viewCart);
        }

        public ActionResult DecreaseQuantity(int id)
        {
            Product product = _productService.GetProductById(id);
            if (product == null)
                throw new Exception();

            List<Cart> cart = (List<Cart>)Session["cart"];
            int index = isExist(id);
            if (index != -1)
            {
                cart[index].Quantity--;
                cart[index].Price = cart[index].Product.Price * cart[index].Quantity;
                if(cart[index].Quantity==0)
                    cart.RemoveAt(index);
            }
            Session["cart"] = cart;

            ViewCartViewModel viewCart = new ViewCartViewModel()
            {
                Count = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.Quantity),
                Carts = Session["cart"] == null ? new List<Cart>() : (List<Cart>)Session["cart"],
            };

            return PartialView("_increaseQuantity", viewCart);
        }

        public ActionResult RemoveCartItem(int id)
        {
            Product product = _productService.GetProductById(id);
            if (product == null)
                throw new Exception();

            List<Cart> cart = (List<Cart>)Session["cart"];
            int index = isExist(id);
            cart.RemoveAt(index);

            Session["cart"] = cart;

            ViewCartViewModel viewCart = new ViewCartViewModel()
            {
                Count = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.Quantity),
                Carts = Session["cart"] == null ? new List<Cart>() : (List<Cart>)Session["cart"],
            };

            return PartialView("_increaseQuantity", viewCart);
        }

        private int isExist(int id)
        {
            List<Cart> cart = (List<Cart>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Product.Id.Equals(id))
                    return i;
            return -1;
        }


        
    }
}