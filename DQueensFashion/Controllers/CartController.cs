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
    [CartSetGlobalVariable]
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IImageService _imageService;
        private readonly IGeneralValuesService _generalValuesService;
        private readonly ICustomerService _customerService;

        public CartController(IProductService productService, ICategoryService categoryService, IImageService imageService,
            IGeneralValuesService generalValuesService,ICustomerService customerService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _imageService = imageService;
            _generalValuesService = generalValuesService;
            _customerService = customerService;
        }
        // GET: Cart
        public ActionResult Index()
        {
            List<Cart> cart = (List<Cart>)Session["cart"];
            ViewCartViewModel viewCart = new ViewCartViewModel()
            {
                Count = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.Quantity),
                Carts = Session["cart"] == null ? new List<Cart>() : (List<Cart>)Session["cart"],
                SubTotal = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.TotalPrice),
            };

            Customer customer = GetLoggedInCustomer();
            if (customer != null)
            {
                if (customer.AvailableSubcriptionDiscount.Value
                  && !customer.UsedSubscriptionDiscount.Value)
                {
                    viewCart.CustomerSubscriptionDiscount = true;
                }

            }
            return View(viewCart);
        }

        public ActionResult ViewCart()
        {
            return RedirectToAction(nameof(ViewCart));
        }

        public ActionResult GetCart()
        {
            ViewBag.CartNumber = GetCartNumber();
            return PartialView("_navbarCartNumber");
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
                    MaxQuantity = product.Quantity,
                });
            }
            Session["cart"] = cart;

            ViewBag.CartNumber = GetCartNumber();
            return PartialView("_navbarCartNumber");
        }

        public ActionResult AddToCartCustomReadyMade(int id=0)
        {
            Product product = _productService.GetProductById(id);
            if (product == null)
                throw new Exception();

            if (!(product.CategoryId == AppConstant.CustomMadeCategoryId 
                || product.CategoryId == AppConstant.ReadyMadeCategoryId))
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
                Bust = product.Bust.HasValue ? product.Bust.Value : false,
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

            return PartialView("_AddToCartCustomReadyMade", cartModel);
        }

        [HttpPost]
        public ActionResult AddToCartCustomReadyMade(Cart cartModel)
        {
            Product product = _productService.GetProductById(cartModel.ProductId);
            if (product == null)
                throw new Exception();

            string mainImage = _imageService.GetImageFilesForProduct(product.Id).Count() < 1
                ? AppConstant.DefaultProductImage
                : _imageService.GetMainImageForProduct(product.Id).ImagePath;

            List<Cart> cart = new List<Cart>();

            int index = isExistCustomReadyMade(cartModel.ProductId,cartModel);
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
                    MaxQuantity = AppConstant.MaxCustomMadeAddToCart,
                    Discount = product.Discount,
                    InitialPrice = product.Price,
                    UnitPrice = product.SubTotal,
                    TotalPrice = product.SubTotal * cartModel.Quantity,
                    MainImage = mainImage,
                    Description = GetCartDescription(cartModel,product.CategoryId),

                    //measurement
                    ShoulderValue = cartModel.ShoulderValue,
                    ArmHoleValue = cartModel.ArmHoleValue,
                    BustValue =cartModel.BustValue,
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
            Customer customer = GetLoggedInCustomer();
            if (customer != null)
            {
                if (customer.AvailableSubcriptionDiscount.Value
                  && !customer.UsedSubscriptionDiscount.Value)
                {
                    viewCart.CustomerSubscriptionDiscount = true;
                }

            }
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

        [HttpGet]
        [Authorize(Roles = AppConstant.CustomerRole)]
        public ActionResult CheckOut()
        {
            List<Cart> cart = (List<Cart>)Session["cart"];
            ViewCartViewModel viewCart = new ViewCartViewModel()
            {
                Count = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.Quantity),
                Carts = Session["cart"] == null ? new List<Cart>() : (List<Cart>)Session["cart"],
                SubTotal = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.TotalPrice),
            };

            if (viewCart.Count < 1)
                return RedirectToAction(nameof(Index));

            foreach (var c in cart)
            {
                var product = _productService.GetProductById(c.Product.Id);
                c.InitialPrice = product.Price;
                c.Discount = product.Discount;
                c.UnitPrice = product.SubTotal;
                c.TotalPrice = product.SubTotal * c.Quantity;
                c.Product = product;
            }

            viewCart.SubTotal = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.TotalPrice);
            Session["cart"] = cart;


            Customer customer = GetLoggedInCustomer();
            if (customer != null)
            {
                if (customer.AvailableSubcriptionDiscount.Value
                  && !customer.UsedSubscriptionDiscount.Value)
                {
                    viewCart.SubDiscountPrice = _productService.
                        CalculateProductPrice(viewCart.SubTotal, _generalValuesService.GetGeneralValues().NewsLetterSubscriptionDiscount);
                    viewCart.CustomerSubscriptionDiscount = true;
                }

            }


            return View(viewCart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles =AppConstant.CustomerRole)]
        public ActionResult CheckOut(ViewCartViewModel cartModel)
        {
            if (!ModelState.IsValid)
            {
                List<Cart> cart = (List<Cart>)Session["cart"];
                ViewCartViewModel viewCart = new ViewCartViewModel()
                {
                    Count = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.Quantity),
                    Carts = Session["cart"] == null ? new List<Cart>() : (List<Cart>)Session["cart"],
                    SubTotal = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.TotalPrice),
                };
                ModelState.AddModelError("", "One or more validation errors");

                if (viewCart.Count < 1)
                    return RedirectToAction(nameof(Index));

                Customer customer = GetLoggedInCustomer();
                if (customer != null)
                {
                    if (customer.AvailableSubcriptionDiscount.Value
                      && !customer.UsedSubscriptionDiscount.Value)
                    {
                        viewCart.SubDiscountPrice = _productService.
                            CalculateProductPrice(viewCart.SubTotal, _generalValuesService.GetGeneralValues().NewsLetterSubscriptionDiscount);
                        viewCart.CustomerSubscriptionDiscount = true;
                    }

                }

                return View(viewCart);
            }

            List<Cart> _cart = (List<Cart>)Session["cart"];
            ViewCartViewModel _viewCart = new ViewCartViewModel()
            {
                Count = Session["cart"] == null ? 0 : ((List<Cart>)Session["cart"]).Sum(c => c.Quantity),
            };

            if (_viewCart.Count < 1)
                return RedirectToAction(nameof(ViewCart));

            Session["Firstname"] = cartModel.FirstName;
            Session["Lastname"] = cartModel.LastName;
            Session["PhoneNumber"] = cartModel.Phone;
            Session["Address"] = cartModel.Address;

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

        private int isExistCustomReadyMade(int id, Cart cartModel)
        {
            if (Session["cart"] == null)
                return -2; //no cart session yet
            List<Cart> cart = (List<Cart>)Session["cart"];
            for (int i = 0; i < cart.Count; i++)
                if (cart[i].Product.Id.Equals(id))
                    if (cart[i].ShoulderValue == cartModel.ShoulderValue
                        && cart[i].ArmHoleValue == cartModel.ArmHoleValue
                        && cart[i].BustValue == cartModel.BustValue
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
            if (id == AppConstant.CustomMadeCategoryId || id==AppConstant.ReadyMadeCategoryId)
            {
                if (cartModel.ShoulderValue > 0)
                    description += "Shoulder : " + cartModel.ShoulderValue +"\"" + "\r\n";

                if (cartModel.ArmHoleValue > 0)
                    description += "Arm hole : " + cartModel.ArmHoleValue + "\"" + "\r\n";

                if (cartModel.BustValue > 0)
                    description += "Bust : " + cartModel.BustValue + "\"" + "\r\n";
              
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
            return description;
        }

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

