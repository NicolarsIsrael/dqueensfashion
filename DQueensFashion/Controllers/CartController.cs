﻿using DQueensFashion.Core.Model;
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

        public ActionResult AddToCart(int id=0,int quantity=1)
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

            ViewBag.CartNumber = GetCartNumber();
            return PartialView("_navbarCartNumber");
        }

        public ActionResult AddToCartReadyMade(int id= 0)
        {
            Product product = _productService.GetProductById(id);
            if (product == null)
                throw new Exception();

            if (product.CategoryId != AppConstant.ReadyMadeCategoryId)
                throw new Exception();

            string mainImage = _imageService.GetImageFilesForProduct(product.Id).Count() < 1
                ? AppConstant.DefaultProductImage
                : _imageService.GetMainImageForProduct(product.Id).ImagePath;

            Cart cartModel = new Cart()
            {
                ProductId = product.Id,
                ProductName = product.Name.Length > 20 ? product.Name.Substring(0, 18) + "..." : product.Name,
                CategoryName = product.Category.Name.Length > 20
                            ? product.Category.Name.Substring(0, 18) + "..." : product.Category.Name,
                MainImage = mainImage,
                Quantity = product.Quantity,
                InitialPrice = product.Price,
                Discount = product.Discount,
                UnitPrice = product.SubTotal,

                ExtraSmallQuantity = product.ExtraSmallQuantity,
                SmallQuantiy = product.SmallQuantiy,
                MediumQuantiy = product.MediumQuantiy,
                LargeQuantity = product.LargeQuantity,
                ExtraLargeQuantity = product.ExtraLargeQuantity,
            };

            return PartialView("_AddToCartReadyMade",cartModel);
        }

        [HttpPost]
        public ActionResult AddToCartReadyMade(Cart cartModel)
        {
            Product product = _productService.GetProductById(cartModel.ProductId);
            if (product == null)
                throw new Exception();

            string mainImage = _imageService.GetImageFilesForProduct(product.Id).Count() < 1
                ? AppConstant.DefaultProductImage
                : _imageService.GetMainImageForProduct(product.Id).ImagePath;

            List<Cart> cart = new List<Cart>();

            int index = isExistReadyMade(cartModel.ProductId,cartModel);
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
                    ReadyMadeSize = cartModel.ReadyMadeSize,
                    Description = GetCartDescription(cartModel,product.CategoryId),
                });
            }
            Session["cart"] = cart;

            ViewBag.CartNumber = GetCartNumber();
            return PartialView("_navbarCartNumber");
        }

        public ActionResult AddToCartCustomMade(int id=0)
        {
            Product product = _productService.GetProductById(id);
            if (product == null)
                throw new Exception();

            if (product.CategoryId != AppConstant.CustomMadeCategoryId)
                throw new Exception();

            string mainImage = _imageService.GetImageFilesForProduct(product.Id).Count() < 1
                ? AppConstant.DefaultProductImage
                : _imageService.GetMainImageForProduct(product.Id).ImagePath;

            Cart cartModel = new Cart()
            {
                ProductId = product.Id,
                ProductName = product.Name.Length > 20 ? product.Name.Substring(0, 18) + "..." : product.Name,
                CategoryName = product.Category.Name.Length > 20 
                            ? product.Category.Name.Substring(0,18) + "..." : product.Category.Name,
                MainImage = mainImage,
                Quantity = product.Quantity,
                InitialPrice = product.Price,
                Discount = product.Discount,
                UnitPrice = product.SubTotal,

                //measurement
                Shoulder = product.Shoulder.HasValue ? product.Shoulder.Value : false,
                ArmHole = product.ArmHole.HasValue ? product.ArmHole.Value : false,
                Burst = product.Burst.HasValue ? product.Burst.Value : false,
                Waist = product.Waist.HasValue ? product.Waist.Value : false,
                Hips = product.Hips.HasValue ? product.Hips.Value : false,
                Thigh = product.Thigh.HasValue ? product.Thigh.Value : false,
                FullBodyLength = product.FullBodyLength.HasValue ? product.FullBodyLength.Value : false,
                KneeGarmentLength = product.KneeGarmentLength.HasValue ? product.KneeGarmentLength.Value : false,
                TopLength = product.TopLength.HasValue ? product.TopLength.Value : false,
                TrousersLength = product.TrousersLength.HasValue ? product.TrousersLength.Value : false,
                RoundAnkle = product.RoundAnkle.HasValue ? product.RoundAnkle.Value : false,
                NipNip = product.NipNip.HasValue ? product.NipNip.Value : false,
                SleeveLength = product.SleeveLength.HasValue ? product.SleeveLength.Value : false,
            };

            return PartialView("_AddToCartCustomMade", cartModel);
        }

        [HttpPost]
        public ActionResult AddToCartCustomMade(Cart cartModel)
        {

            Product product = _productService.GetProductById(cartModel.ProductId);
            if (product == null)
                throw new Exception();

            string mainImage = _imageService.GetImageFilesForProduct(product.Id).Count() < 1
                ? AppConstant.DefaultProductImage
                : _imageService.GetMainImageForProduct(product.Id).ImagePath;

            List<Cart> cart = new List<Cart>();

            int index = isExistCustomMade(cartModel.ProductId,cartModel);
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
                    Description = GetCartDescription(cartModel,product.CategoryId),

                    //measurement
                    ShoulderValue = cartModel.ShoulderValue,
                    ArmHoleValue = cartModel.ArmHoleValue,
                    BurstValue =cartModel.BurstValue,
                    WaistValue = cartModel.WaistValue,
                    HipsValue = cartModel.HipsValue,
                    ThighValue = cartModel.ThighValue,
                    FullBodyLengthValue = cartModel.FullBodyLengthValue,
                    KneeGarmentLengthValue = cartModel.KneeGarmentLengthValue,
                    TopLengthValue = cartModel.TopLengthValue,
                    TrousersLengthValue = cartModel.TrousersLengthValue,
                    RoundAnkleValue = cartModel.RoundAnkleValue,
                    NipNipValue = cartModel.NipNipValue,
                    SleeveLengthValue = cartModel.SleeveLengthValue,
                });
            }
            Session["cart"] = cart;

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

        public ActionResult ChangeCartItemQuantity(int id,int quantity)
        {
            Product product = _productService.GetProductById(id);
            if (product == null)
                throw new Exception();

            List<Cart> cart = (List<Cart>)Session["cart"];
            int index = isExist(id);
            if (index != -1)
            {
                cart[index].Quantity = quantity;
                cart[index].UnitPrice = cart[index].Product.SubTotal;
                cart[index].TotalPrice = cart[index].Product.SubTotal * quantity;
                if (product.CategoryId != AppConstant.CustomMadeCategoryId
                    && product.CategoryId != AppConstant.ReadyMadeCategoryId)
                {
                    cart[index].Description = cart[index].Quantity > 1
                    ? cart[index].Quantity.ToString() + " Pieces"
                    : cart[index].Quantity.ToString() + " Piece";
                }

                if (cart[index].Quantity == 0)
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

        private int isExistReadyMade(int id,Cart cartModel)
        {
            if (Session["cart"] == null)
                return -2; //no cart session yet
            List<Cart> cart = (List<Cart>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Product.Id.Equals(id))
                    if(string.Compare(cart[i].ReadyMadeSize, cartModel.ReadyMadeSize, true) == 0)
                        return i; //item already exist in cart
            return -1; //cart session is available but item not in cart
        }

        private int isExistCustomMade(int id, Cart cartModel)
        {
            if (Session["cart"] == null)
                return -2; //no cart session yet
            List<Cart> cart = (List<Cart>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Product.Id.Equals(id))
                    if (cart[i].ShoulderValue == cartModel.ShoulderValue
                        && cart[i].ArmHoleValue == cartModel.ArmHoleValue
                        && cart[i].BurstValue == cartModel.BurstValue
                        && cart[i].WaistValue == cartModel.WaistValue
                        && cart[i].HipsValue == cartModel.HipsValue
                        && cart[i].ThighValue == cartModel.ThighValue
                        && cart[i].FullBodyLengthValue == cartModel.FullBodyLengthValue
                        && cart[i].KneeGarmentLengthValue == cartModel.KneeGarmentLengthValue
                        && cart[i].TopLengthValue == cartModel.TopLengthValue
                        && cart[i].TrousersLengthValue == cartModel.TrousersLengthValue
                        && cart[i].RoundAnkleValue == cartModel.RoundAnkleValue
                        && cart[i].NipNipValue == cartModel.NipNipValue
                        && cart[i].SleeveLengthValue == cartModel.SleeveLengthValue)
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

        private string GetCartDescription(Cart cartModel,int id)
        {
            string description = "";
            if (id == AppConstant.CustomMadeCategoryId)
            {
                if (cartModel.ShoulderValue > 0)
                    description += "Shoulder : " + cartModel.ShoulderValue +"\"" + "\r\n";

                if (cartModel.ArmHoleValue > 0)
                    description += "Arm hole : " + cartModel.ArmHoleValue + "\"" + "\r\n";

                if (cartModel.BurstValue > 0)
                    description += "Burst : " + cartModel.BurstValue + "\"" + "\r\n";
              
                if (cartModel.WaistValue > 0)
                    description += "Waist : " + cartModel.WaistValue + "\"" + "\r\n";

                if (cartModel.HipsValue > 0)
                    description += "Hips : " + cartModel.HipsValue + "\"" + "\r\n";

                if (cartModel.ThighValue > 0)
                    description += "Thigh : " + cartModel.ThighValue + "\"" + "\r\n";

                if (cartModel.FullBodyLengthValue > 0)
                    description += "Full body : " + cartModel.FullBodyLengthValue + "\"" + "\r\n";

                if (cartModel.KneeGarmentLengthValue > 0)
                    description += "Knee garment : " + cartModel.KneeGarmentLengthValue + "\"" + "\r\n";

                if (cartModel.TopLengthValue > 0)
                    description += "Top length : " + cartModel.TopLengthValue + "\"" + "\r\n";

                if (cartModel.TrousersLengthValue > 0)
                    description += "Trousers length: " + cartModel.TrousersLengthValue + "\"" + "\r\n";

                if (cartModel.RoundAnkleValue > 0)
                    description += "Round neck : " + cartModel.RoundAnkleValue + "\"" + "\r\n";

                if (cartModel.NipNipValue > 0)
                    description += "Nip-Nip : " + cartModel.NipNipValue + "\"" + "\r\n";

                if (cartModel.SleeveLengthValue > 0)
                    description += "Sleeve : " + cartModel.SleeveLengthValue + "\"" + "\r\n";

            }
            else if (id == AppConstant.ReadyMadeCategoryId)
            {
                description = cartModel.ReadyMadeSize;
            }
            return description;
        }
        #endregion
    }
}

