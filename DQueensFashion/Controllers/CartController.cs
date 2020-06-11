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
    [CartSetGlobalVariable]
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IImageService _imageService;

        public CartController(IProductService productService, ICategoryService categoryService, IImageService imageService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _imageService = imageService;
        }
        // GET: Cart
        public ActionResult Index()
        {
            return RedirectToAction(nameof(ViewCart));
        }

        public ActionResult AddToCart(int id,int quantity=1)
        {
            Product product = _productService.GetProductById(id);
            if (product == null)
                throw new Exception();

            string mainImage = _imageService.GetImageFilesForProduct(product.Id).Count() < 1
                ? AppConstant.DefaultProductImage
                : _imageService.GetMainImageForProduct(product.Id).ImagePath;

            if (Session["cart"] == null)
            {
                List<Cart> cart = new List<Cart>();
                cart.Add(new Cart
                {
                    Product = product,
                    Quantity = quantity,
                    Price = product.Price * quantity,
                    MainImage = mainImage,
                });
                Session["cart"] = cart;
            }
            else
            {
                List<Cart> cart = (List<Cart>)Session["cart"];
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Quantity+=quantity;
                    cart[index].Price = cart[index].Product.Price * cart[index].Quantity;
                }
                else
                {
                    cart.Add(new Cart
                    {
                        Product = product,
                        Quantity = quantity,
                        Price = product.Price * quantity,
                        MainImage = mainImage,
                    });
                }
                Session["cart"] = cart;
            }

            ViewCartViewModel viewCart = new ViewCartViewModel()
            {
                Count = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.Quantity),
                Carts = Session["cart"] == null ? new List<Cart>() : (List<Cart>)Session["cart"],
            };
            ViewBag.CartNumber = GetCartNumber();
            return PartialView("_navbarCartNumber");
        }

        public ActionResult GetCart()
        {
            ViewBag.CartNumber = GetCartNumber();
            return PartialView("_navbarCartNumber");
        }

        public ActionResult ViewCart()
        {
            List<Cart> cart = (List<Cart>)Session["cart"];
            ViewCartViewModel viewCart = new ViewCartViewModel()
            {
                Count = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.Quantity),
                Carts = Session["cart"] == null ? new List<Cart>() : (List<Cart>)Session["cart"],
                SubTotal= Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.Price),
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
                SubTotal = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.Price),
            };

            return PartialView("_cartTable",viewCart);
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
                SubTotal = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.Price),
            };

            return PartialView("_cartTable", viewCart);
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
                SubTotal = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.Price),
            };

            return PartialView("_cartTable", viewCart);
        }

        private int isExist(int id)
        {
            List<Cart> cart = (List<Cart>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Product.Id.Equals(id))
                    return i;
            return -1;
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