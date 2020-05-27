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
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        // GET: Product
        public ActionResult Index()
        {

            IEnumerable<ViewProductsViewModel> products = _productService.GetAllProducts()
                .Select(p => new ViewProductsViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description.Length > 35 ? p.Description.Substring(0, 35) + "..." : p.Description,
                    Image1 = p.ImagePath1,
                    Quantity = p.Quantity.ToString(),
                    Price = p.Price.ToString(),

                }).ToList();

            ProductIndexViewModel productIndex = new ProductIndexViewModel()
            {
                Products = products,
            };

            return View(productIndex);
        }

        public ActionResult SearchProduct(string searchString, int sortString)
        {
            try
            {
                IEnumerable<Product> _products = _productService.GetAllProducts().ToList();
                if (!string.IsNullOrEmpty(searchString))
                    _products = _products.Where(p => p.Name.ToLower().Contains(searchString.ToLower())
                    || p.Tags.ToLower().Contains(searchString.ToLower())
                    || p.Description.ToLower().Contains(searchString.ToLower())).ToList();

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


                return PartialView("_productsList", products);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public ActionResult ProductDetails(int id)
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
                DateCreatedString = product.DateCreated.ToString("dd-MMM-yyyy : hh-mm-ss"),
                Image1 = string.IsNullOrEmpty(product.ImagePath1) ? "" : product.ImagePath1,
                Image2 = string.IsNullOrEmpty(product.ImagePath2) ? "" : product.ImagePath2,
                Image3 = string.IsNullOrEmpty(product.ImagePath3) ? "" : product.ImagePath3,
                Image4 = string.IsNullOrEmpty(product.ImagePath4) ? "" : product.ImagePath4,
            };

            return View(productModel);
        }
    }
}