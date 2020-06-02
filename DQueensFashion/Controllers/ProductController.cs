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
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService, ICustomerService customerService, IOrderService orderService, ICategoryService categoryService)
        {
            _productService = productService;
            _customerService = customerService;
            _orderService = orderService;
            _categoryService = categoryService;
        }
        // GET: Product
        public ActionResult Index(int categoryId=0)
        {
            IEnumerable<Product> _products = _productService.GetAllProducts().ToList();
           
            if (categoryId > 0)
                _products = _products.Where(p => p.Category.Id == categoryId);

            IEnumerable<ViewProductsViewModel> products = _products
                .Select(p => new ViewProductsViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description.Length > 35 ? p.Description.Substring(0, 35) + "..." : p.Description,
                    Image1 = p.ImagePath1,
                    Quantity = p.Quantity.ToString(),
                    Price = p.Price.ToString(),

                }).ToList();

            //pagination
            ViewBag.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)products.Count() / AppConstant.ProductIndexPageSize));
            ViewBag.CurrentPage = 1;
            products = products.Skip(AppConstant.ProductIndexPageSize * 0).Take(AppConstant.ProductIndexPageSize).ToList();


            ProductIndexViewModel productIndex = new ProductIndexViewModel()
            {
                Products = products,
                Categories = _categoryService.GetAllCategories()
                    .Select(c => new CategoryNameAndId()
                    {
                        Id = c.Id,
                        Name = c.Name,
                    }).OrderBy(c=>c.Name).ToList(),
            };

            foreach(var category in productIndex.Categories)
            {
                if (category.Id == categoryId)
                    category.Selected = "selected";
            }


            return View(productIndex);
        }

        public ActionResult SearchProduct(string searchString, int sortString, int categoryId=0)
        {
            try
            {
                IEnumerable<Product> _products = _productService.GetAllProducts().ToList();
                if (!string.IsNullOrEmpty(searchString))
                    _products = _products.Where(p => p.Name.ToLower().Contains(searchString.ToLower())
                    || p.Tags.ToLower().Contains(searchString.ToLower())
                    || p.Description.ToLower().Contains(searchString.ToLower())).ToList();

                if (categoryId > 0)
                    _products = _products.Where(p => p.Category.Id == categoryId);

                IEnumerable<ViewProductsViewModel> products = _products
                    .Select(p => new ViewProductsViewModel()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description.Length > 35 ? p.Description.Substring(0, 35) + "..." : p.Description,
                        Image1 = p.ImagePath1,
                        Quantity = p.Quantity.ToString(),
                        Price = p.Price.ToString(),
                        DateCreated= p.DateCreated,
                    }).ToList();

                //sort
                switch (sortString)
                {
                    //sort by best selling
                    case 1:
                        break;

                    //alphabetically a-z
                    case 2:
                        products = products.OrderBy(p => p.Name);
                        break;

                    //alphabetically z-a
                    case 3:
                        products = products.OrderByDescending(p => p.Name);
                        break;

                    //price low to high
                    case 4:
                        products = products.OrderBy(p => p.Price);
                        break;

                    //price high to low
                    case 5:
                        products = products.OrderByDescending(p => p.Price);
                        break;

                    //date new to old
                    case 6:
                        products = products.OrderByDescending(p => p.DateCreated);
                        break;

                    //date old to new
                    case 7:
                        products = products.OrderBy(p => p.DateCreated);
                        break;


                }

                //pagination
                ViewBag.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)products.Count() / AppConstant.ProductIndexPageSize));
                ViewBag.CurrentPage = 1;
                products = products.Skip(AppConstant.ProductIndexPageSize * 0).Take(AppConstant.ProductIndexPageSize).ToList();

                return PartialView("_productsList", products);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public ActionResult ProductPagination(string searchString, int sortString, int categoryId = 0, int pageNumber = 1)
        {
            try
            {
                IEnumerable<Product> _products = _productService.GetAllProducts().ToList();
                if (!string.IsNullOrEmpty(searchString))
                    _products = _products.Where(p => p.Name.ToLower().Contains(searchString.ToLower())
                    || p.Tags.ToLower().Contains(searchString.ToLower())
                    || p.Description.ToLower().Contains(searchString.ToLower())).ToList();

                if (categoryId > 0)
                    _products = _products.Where(p => p.Category.Id == categoryId);

                IEnumerable<ViewProductsViewModel> products = _products
                    .Select(p => new ViewProductsViewModel()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description.Length > 35 ? p.Description.Substring(0, 35) + "..." : p.Description,
                        Image1 = p.ImagePath1,
                        Quantity = p.Quantity.ToString(),
                        Price = p.Price.ToString(),
                        DateCreated = p.DateCreated,
                    }).ToList();

                //sort
                switch (sortString)
                {
                    //sort by best selling
                    case 1:
                        break;

                    //alphabetically a-z
                    case 2:
                        products = products.OrderBy(p => p.Name);
                        break;

                    //alphabetically z-a
                    case 3:
                        products = products.OrderByDescending(p => p.Name);
                        break;

                    //price low to high
                    case 4:
                        products = products.OrderBy(p => p.Price);
                        break;

                    //price high to low
                    case 5:
                        products = products.OrderByDescending(p => p.Price);
                        break;

                    //date new to old
                    case 6:
                        products = products.OrderByDescending(p => p.DateCreated);
                        break;

                    //date old to new
                    case 7:
                        products = products.OrderBy(p => p.DateCreated);
                        break;


                }

                //pagination
                if (pageNumber < 1)
                    pageNumber = 1;
                ViewBag.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)products.Count() / AppConstant.ProductIndexPageSize));
                ViewBag.CurrentPage = pageNumber;
                products = products.Skip(AppConstant.ProductIndexPageSize * (pageNumber - 1)).Take(AppConstant.ProductIndexPageSize).ToList();

                return PartialView("_productsList", products);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public ActionResult ProductDetails(int id=0)
        {
            Product product = _productService.GetProductById(id);
            if (product == null)
                return HttpNotFound();

            ViewProductsViewModel productModel = new ViewProductsViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price.ToString(),
                Quantity = product.Quantity.ToString(),
                Category = product.Category.Name,
                Tags = product.Tags,
                DateCreatedString = product.DateCreated.ToString("dd/MMM/yyyy : hh-mm-ss"),
                Image1 = string.IsNullOrEmpty(product.ImagePath1) ? "" : product.ImagePath1,
                Image2 = string.IsNullOrEmpty(product.ImagePath2) ? "" : product.ImagePath2,
                Image3 = string.IsNullOrEmpty(product.ImagePath3) ? "" : product.ImagePath3,
                Image4 = string.IsNullOrEmpty(product.ImagePath4) ? "" : product.ImagePath4,
            };

            return View(productModel);
        }


        [Authorize(Roles = AppConstant.CustomerRole)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BuyProduct(int id,int quantity)
        {
            Product product = _productService.GetProductById(id);
            if (product == null || quantity<1)
                throw new Exception();

            Customer customer = GetLoggedInCustomer();
            if (customer == null)
                throw new Exception();

            if (product.Quantity < quantity)
                throw new Exception();

            product.Quantity = product.Quantity - quantity;
            LineItem lineItem = new LineItem()
            {
                Product = product,
                Quantity = quantity,
                TotalAmount = product.Price * quantity,
                DateCreated=DateTime.Now,
                DateModified= DateTime.Now,
            };
            List<LineItem> lineItems = new List<LineItem>();
            lineItems.Add(lineItem);

            Order order = new Order()
            {
                Customer = customer,
                LineItems = lineItems,
                TotalAmount = lineItems.Sum(l => l.TotalAmount),
                TotalQuantity = lineItems.Sum(l => l.Quantity),
                OrderStatus=  OrderStatus.Processing,
            };

            _orderService.CreateOrder(order);
            return RedirectToAction("Index");
        }

        public ActionResult ProductQuickView(int id)
        {
            Product product = _productService.GetProductById(id);
            if (product == null)
                throw new Exception();

            ViewProductsViewModel productModel = new ViewProductsViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Image1 = product.ImagePath1,
                Image2 = product.ImagePath2,
                Image3 = product.ImagePath3,
                Image4 = product.ImagePath4,
                Category = product.Category.Name,
                Price = product.Price.ToString(),
            };

            return PartialView("_productQuickView", productModel);
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