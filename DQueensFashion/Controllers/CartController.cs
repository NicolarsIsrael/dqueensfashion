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

            List<Cart> cart = new List<Cart>();
            int index = isExist(id);
            if (index > -1)
            {
                cart = (List<Cart>)Session["cart"];
                cart[index].Quantity += quantity;
                cart[index].UnitPrice = cart[index].Product.SubTotal;
                cart[index].TotalPrice = cart[index].Product.SubTotal * cart[index].Quantity;
                cart[index].Description = cart[index].Quantity > 1
                    ? cart[index].Quantity.ToString() + " Pieces"
                    : cart[index].Quantity.ToString() + " Piece";
            }
            else
            {
                if (index == -1)
                    cart = (List<Cart>)Session["cart"];
                cart.Add(new Cart
                {
                    Product = product,
                    Quantity = quantity,
                    Discount = product.Discount,
                    InitialPrice = product.Price,
                    UnitPrice = product.SubTotal,
                    TotalPrice = product.SubTotal * quantity,
                    MainImage = mainImage,
                    Description = quantity > 1 ? quantity.ToString() + " Pieces" : quantity.ToString() + " Piece",
                });
            }
            Session["cart"] = cart;

            ViewCartViewModel viewCart = new ViewCartViewModel()
            {
                Count = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.Quantity),
                Carts = Session["cart"] == null ? new List<Cart>() : (List<Cart>)Session["cart"],
            };
            ViewBag.CartNumber = GetCartNumber();
            return PartialView("_navbarCartNumber");
        }

        public ActionResult AddToCartCustomMade(int id)
        {
            Product product = _productService.GetProductById(id);
            if (product == null)
                throw new Exception();

            if (product.CategoryId != AppConstant.CustomMadeCategoryId)
                throw new Exception();

            AddToCartCustomMade productModel = new AddToCartCustomMade()
            {
                ProductId=product.Id,
                ProductName = product.Name,
                Quantity=product.Quantity,
                WaistLength = product.WaistLength.HasValue ? product.WaistLength.Value : false,
                ShoulderLength = product.ShoulderLength.HasValue ? product.ShoulderLength.Value : false,
                BurstSize = product.BurstSize.HasValue ? product.BurstSize.Value : false,
            };

            return PartialView("_AddToCartCustomMade", productModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToCartCustomMade(AddToCartCustomMade cartModel)
        {

            Product product = _productService.GetProductById(cartModel.ProductId);
            if (product == null)
                throw new Exception();

            string mainImage = _imageService.GetImageFilesForProduct(product.Id).Count() < 1
                ? AppConstant.DefaultProductImage
                : _imageService.GetMainImageForProduct(product.Id).ImagePath;

            List<Cart> cart = new List<Cart>();

            int index = isExist(cartModel.ProductId);
            if (index > -1)
            {
                cart = (List<Cart>)Session["cart"];
                cart[index].Quantity += cartModel.Quantity;
                cart[index].UnitPrice = cart[index].Product.SubTotal;
                cart[index].TotalPrice = cart[index].Product.SubTotal * cart[index].Quantity;
            }
            else
            {
                if (index == -1)
                    cart = (List<Cart>)Session["cart"];
                cart.Add(new Cart
                {
                    Product = product,
                    Quantity = cartModel.Quantity,
                    Discount = product.Discount,
                    InitialPrice = product.Price,
                    UnitPrice = product.SubTotal,
                    TotalPrice = product.SubTotal * cartModel.Quantity,
                    MainImage = mainImage,
                    Description = GetCartDescription(cartModel),
                });
            }
            Session["cart"] = cart;

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
                SubTotal= Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.TotalPrice),
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
                cart[index].UnitPrice = cart[index].Product.SubTotal;
                cart[index].TotalPrice = cart[index].Product.SubTotal * cart[index].Quantity;
                if (product.CategoryId != AppConstant.CustomMadeCategoryId)
                {
                    cart[index].Description = cart[index].Quantity > 1
                    ? cart[index].Quantity.ToString() + " Pieces"
                    : cart[index].Quantity.ToString() + " Piece";
                }
            }
            Session["cart"] = cart;

            ViewCartViewModel viewCart = new ViewCartViewModel()
            {
                Count = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.Quantity),
                Carts = Session["cart"] == null ? new List<Cart>() : (List<Cart>)Session["cart"],
                SubTotal = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.TotalPrice),
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
                cart[index].UnitPrice = cart[index].Product.SubTotal;
                cart[index].TotalPrice = cart[index].Product.SubTotal * cart[index].Quantity;
                if (product.CategoryId != AppConstant.CustomMadeCategoryId)
                {
                    cart[index].Description = cart[index].Quantity > 1
                    ? cart[index].Quantity.ToString() + " Pieces"
                    : cart[index].Quantity.ToString() + " Piece";
                }

                if (cart[index].Quantity==0)
                    cart.RemoveAt(index);
            }
            Session["cart"] = cart;

            ViewCartViewModel viewCart = new ViewCartViewModel()
            {
                Count = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.Quantity),
                Carts = Session["cart"] == null ? new List<Cart>() : (List<Cart>)Session["cart"],
                SubTotal = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.TotalPrice),
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
                SubTotal = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.TotalPrice),
            };

            return PartialView("_cartTable", viewCart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut()
        {
            List<Cart> cart = (List<Cart>)Session["cart"];
            ViewCartViewModel viewCart = new ViewCartViewModel()
            {
                Count = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.Quantity),
                
            };

            if (viewCart.Count < 1)
                return RedirectToAction(nameof(ViewCart));

            return RedirectToAction("PaymentWithPaypal", "Payment");
        }


        #region private functions

        private int isExist(int id)
        {
            if (Session["cart"] == null)
                return -2; //no cart session yet
            List<Cart> cart = (List<Cart>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Product.Id.Equals(id))
                    return i; //item already exist in cart
            return -1; //cart session is available but item not in cart
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

        private string GetCartDescription(AddToCartCustomMade cartModel)
        {
            string description = "";
            if (cartModel.ShoulderLengthValue > 0)
                description += "Shoulder length : " + cartModel.ShoulderLengthValue + "\r\n";

            if (cartModel.WaistLengthValue > 0)
                description += "Waist length : " + cartModel.WaistLengthValue + "\r\n";

            if (cartModel.BurstSizeValue > 0)
                description += "Burst size : " + cartModel.BurstSizeValue + "\r\n";

            return description;
        }
        #endregion
    }
}