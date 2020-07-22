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
    [ProductSetGlobalVariable]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        private readonly IOrderService _orderService;
        private readonly ICategoryService _categoryService;
        private readonly IReviewService _reviewService;
        private readonly IImageService _imageService;
        private readonly ILineItemService _lineItemService;
        private GeneralService generalService;

        public ProductController(IProductService productService, ICustomerService customerService, IOrderService orderService, ICategoryService categoryService,
            IReviewService reviewService, IImageService imageService,ILineItemService lineItemService)
        {
            _productService = productService;
            _customerService = customerService;
            _orderService = orderService;
            _categoryService = categoryService;
            _reviewService = reviewService;
            _imageService = imageService;
            _lineItemService = lineItemService;
            generalService = new GeneralService();
        }
        // GET: Product
        public ActionResult Index()
        {
            return RedirectToAction(nameof(Shop));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchShop(string query)
        {
            try
            {
                return RedirectToAction(nameof(Shop), new { query = query });
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ActionResult Shop(string query = "",int categoryId = 0,int sort=0,int pageNumber=1)
        {
            var allImages = _imageService.GetAllImageMainFiles();
            IEnumerable<Product> _products = _productService.GetAllProducts().ToList();

            if (!string.IsNullOrEmpty(query))
            {
                _products = _products.Where(p => p.Name.ToLower().Contains(query.ToLower())
                    || p.Tags.ToLower().Contains(query.ToLower())
                    || p.Category.Name.ToLower().Contains(query.ToLower())
                    || p.Description.ToLower().Contains(query.ToLower())).ToList();
                ViewBag.Query = query;
            }
                
            if (categoryId > 0)
                _products = _products.Where(p => p.Category.Id == categoryId);

            IEnumerable<ViewProductsViewModel> products = _products
                .Select(p => new ViewProductsViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    GeneratedUrl = generalService.GenerateItemNameAsParam(p.Id, p.Name),
                    MainImage = allImages.Where(image => image.ProductId == p.Id).Count() < 1 ?
                        AppConstant.DefaultProductImage :
                        allImages.Where(image => image.ProductId == p.Id).FirstOrDefault().ImagePath,
                    Price = p.Price,
                    Discount = p.Discount,
                    SubTotal = p.SubTotal,
                    Category = p.Category.Name,
                    CategoryId = p.CategoryId,
                    DateCreated = p.DateCreatedUtc,
                    Rating = new RatingViewModel()
                    {
                        AverageRating = p.AverageRating.ToString("0.0"),
                        IsDouble = (p.AverageRating % 1) == 0 ? false : true,
                        FloorAverageRating = (int)Math.Floor(p.AverageRating)
                    },
                    NumberOfOrders = p.NumberOfItemsBought,
                    IsNew = _productService.CheckIfProductIsNew(p.DateCreatedUtc),
                    IsOutOfStock = p.Quantity < 1 ? true : false,
                    LazyLoad = true,
                }).OrderByDescending(p=>p.DateCreated).ToList();

            //sort
            switch (sort)
            {
                //sort by best deals
                case AppConstant.BestDeals:
                    products = products.OrderByDescending(p => p.Discount);
                    break;

                //sort by best selling
                case AppConstant.BestSelling:
                    products = products.OrderByDescending(p => p.NumberOfOrders);
                    break;

                //alphabetically a-z
                case AppConstant.AlphabeticallyAZ:
                    products = products.OrderBy(p => p.Name);
                    break;

                //alphabetically z-a
                case AppConstant.AlphabeticallyZA:
                    products = products.OrderByDescending(p => p.Name);
                    break;

                //price low to high
                case AppConstant.PriceLowToHigh:
                    products = products.OrderBy(p => p.SubTotal);
                    break;

                //price high to low
                case AppConstant.PriceHighToLow:
                    products = products.OrderByDescending(p => p.SubTotal);
                    break;

                //date new to old
                case AppConstant.MostRecent:
                    products = products.OrderByDescending(p => p.DateCreated);
                    break;

                //date old to new
                case AppConstant.LeastRecent:
                    products = products.OrderBy(p => p.DateCreated);
                    break;

                case AppConstant.HighestRating:
                    products = products.OrderByDescending(p => p.Rating.AverageRating);
                    break;

                case AppConstant.LowestRating:
                    products = products.OrderBy(p => p.Rating.AverageRating);
                    break;

                default:
                    products = products.OrderByDescending(p => p.DateCreated);
                    break;
            }

            //pagination
            if (pageNumber < 1)
                pageNumber = 1;
            ViewBag.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)products.Count() / AppConstant.ProductIndexPageSize));
            ViewBag.CurrentPage = pageNumber;
            products = products.Skip(AppConstant.ProductIndexPageSize * (pageNumber - 1)).Take(AppConstant.ProductIndexPageSize).ToList();

            //dont lazy load top images
            products.Take(8).ToList().ForEach(p => p.LazyLoad = false);

            ProductIndexViewModel productIndex = new ProductIndexViewModel()
            {
                Products = products,
                Categories = _categoryService.GetAllCategories()
                    .Select(c => new CategoryNameAndId()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Selected = c.Id == categoryId ? true : false,
                    }).OrderBy(c => c.Name).ToList(),
                SortValue = sort,
            };
            
            return View(productIndex);
        }

        public ActionResult ProductDetails(int id=0)
        {
            Product product = _productService.GetProductById(id);
            if (product == null)
                return HttpNotFound();

            var allProductImages = _imageService.GetImageFilesForProduct(product.Id);
            var productImages = SetProductImages(allProductImages);

            ViewProductsViewModel productDetails = new ViewProductsViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Discount = product.Discount,
                SubTotal = product.SubTotal,
                Quantity = product.Quantity,
                CategoryId = product.Category.Id,
                Category = product.Category.Name,
                DeliveryDaysDuration = product.DeliveryDaysDuration,
                Tags = product.Tags,
                DateCreatedString = generalService.GetDateInString(product.DateCreated, true, true),
                MainImage = allProductImages.Count() < 1 ?
                        AppConstant.DefaultProductImage :
                        _imageService.GetMainImageForProduct(product.Id).ImagePath,
                OtherImagePaths = productImages,
                IsOutOfStock = product.Quantity < 1 ? true : false,
                Rating = new RatingViewModel()
                {
                    AverageRating = product.AverageRating.ToString("0.0"),
                    TotalReviewCount = _reviewService.GetReviewCountForProduct(product.Id).ToString(),
                    IsDouble = (product.AverageRating % 1) == 0 ? false : true,
                    FloorAverageRating = (int)Math.Floor(product.AverageRating)
                },

                Reviews = _reviewService.GetAllReviewsForProduct(product.Id).ToList()
                    .Select(r => new ViewReviewViewModel()
                    {
                        ReviewId = r.Id,
                        Name = r.Name,
                        Email = r.Email,
                        Comment = r.Comment,
                        Rating = r.Rating,
                        DateCreated = generalService.GetDateInString(r.DateCreated),
                        DateOrder = r.DateCreated,
                    }).OrderByDescending(r => r.DateOrder).ToList(),
                Waist = product.Waist.HasValue ? product.Waist.Value : false,
                Shoulder = product.Shoulder.HasValue ? product.Shoulder.Value : false,
                Bust = product.Bust.HasValue ? product.Bust.Value : false,
            };

            var allImages = _imageService.GetAllImageMainFiles();

            IEnumerable<ViewProductsViewModel> relatedProducts = _productService.GetRelatedProducts(product.Id, product.CategoryId)
                .Select(p => new ViewProductsViewModel()
                {
                    GeneratedUrl = generalService.GenerateItemNameAsParam(p.Id, p.Name),
                    Id = p.Id,
                    Name = p.Name,
                    MainImage = allImages.Where(image => image.ProductId == p.Id).Count() < 1 ?
                        AppConstant.DefaultProductImage :
                        allImages.Where(image => image.ProductId == p.Id).FirstOrDefault().ImagePath,
                    Price = p.Price,
                    Discount = p.Discount,
                    SubTotal = p.SubTotal,
                    Category = p.Category.Name,
                    CategoryId = product.Category.Id,
                    DateCreated = p.DateCreatedUtc,
                    Rating = new RatingViewModel()
                    {
                        AverageRating = p.AverageRating.ToString("0.0"),
                        IsDouble = (p.AverageRating % 1) == 0 ? false : true,
                        FloorAverageRating = (int)Math.Floor(p.AverageRating)
                    },
                    IsNew = _productService.CheckIfProductIsNew(p.DateCreatedUtc),
                    IsOutOfStock = p.Quantity < 1 ? true : false,
                }).OrderByDescending(p=>p.DateCreated).ToList();

            ProductDetailsViewModel productModel = new ProductDetailsViewModel()
            {
                Product = productDetails,
                RelatedProducts = relatedProducts.Take(8),
            };

            //pagination
            ViewBag.ProductId = product.Id;
            ViewBag.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)productModel.Product.Reviews.Count() / AppConstant.ReviewsPageSize));
            ViewBag.CurrentPage = 1;
            productModel.Product.Reviews = productModel.Product.Reviews
                                            .Skip(AppConstant.ReviewsPageSize * 0)
                                            .Take(AppConstant.ReviewsPageSize).ToList();

            return View(productModel);
        }

        public PartialViewResult ReviewPagination(int productId,int sortId, int pageNumber = 1)
        {
            Product product = _productService.GetProductById(productId);
            if (product == null)
                throw new Exception();

            IEnumerable<ViewReviewViewModel> reviews = _reviewService.GetAllReviewsForProduct(product.Id)
                   .Select(r => new ViewReviewViewModel()
                   {
                       ReviewId = r.Id,
                       Name = r.Name,
                       Email = r.Email,
                       Comment = r.Comment,
                       Rating = r.Rating,
                       DateCreated = generalService.GetDateInString(r.DateCreated),
                       DateOrder = r.DateCreatedUtc,
                   }).OrderByDescending(r => r.DateOrder).ToList();

            switch (sortId)
            {
                case 1:
                    reviews = reviews.OrderByDescending(r => r.DateOrder);
                    break;

                case 2:
                    reviews = reviews.OrderBy(r => r.DateOrder);
                    break;

                case 3:
                    reviews = reviews.OrderByDescending(r => r.Rating);
                    break;

                case 4:
                    reviews = reviews.OrderBy(r => r.Rating);
                    break;

                case 5:
                    break;
                    //order by most helpful
            }

            //pagination
            if (pageNumber < 1)
                pageNumber = 1;
            ViewBag.ProductId = product.Id;
            ViewBag.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)reviews.Count() / AppConstant.ReviewsPageSize));
            ViewBag.CurrentPage = pageNumber;
            reviews = reviews.Skip(AppConstant.ReviewsPageSize * (pageNumber - 1)).Take(AppConstant.ReviewsPageSize).ToList();

            return PartialView("_productReview", reviews);
        }

        public PartialViewResult SortReview(int productId, int sortId)
        {
            Product product = _productService.GetProductById(productId);
            if (product == null)
                throw new Exception();

            IEnumerable<ViewReviewViewModel> reviews = _reviewService.GetAllReviewsForProduct(product.Id)
                   .Select(r => new ViewReviewViewModel()
                   {
                       ReviewId = r.Id,
                       Name = r.Name,
                       Email = r.Email,
                       Comment = r.Comment,
                       Rating = r.Rating,
                       DateCreated = generalService.GetDateInString(r.DateCreated),
                       DateOrder = r.DateCreated,
                   }).OrderByDescending(r=>r.DateOrder).ToList();

            switch (sortId)
            {
                case 1:
                    reviews = reviews.OrderByDescending(r => r.DateOrder);
                    break;

                case 2:
                    reviews = reviews.OrderBy(r => r.DateOrder);
                    break;

                case 3:
                    reviews = reviews.OrderByDescending(r => r.Rating);
                    break;

                case 4:
                    reviews = reviews.OrderBy(r => r.Rating);
                    break;

                case 5:
                    break;
                    //order by most helpful
            }

            //pagination
            ViewBag.ProductId = product.Id;
            ViewBag.NumberOfPages = Convert.ToInt32(Math.Ceiling((double)reviews.Count() / AppConstant.ReviewsPageSize));
            ViewBag.CurrentPage = 1;
            reviews = reviews.Skip(AppConstant.ReviewsPageSize * 0)
                                            .Take(AppConstant.ReviewsPageSize).ToList();

            return PartialView("_productReview", reviews);
        }

     
        public ActionResult ProductQuickView(int id=0)
        {
            Product product = _productService.GetProductById(id);
            if (product == null)
                throw new Exception();

            //var productImages = SetProductImages(product.ImagePath2, product.ImagePath3, product.ImagePath4);

            ViewProductsViewModel productModel = new ViewProductsViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                //MainImage = string.IsNullOrEmpty(product.ImagePath1) ? AppConstant.DefaultProductImage : product.ImagePath1,
                //OtherImagePaths = productImages,
                Category = product.Category.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                Rating = new RatingViewModel()
                {
                    AverageRating = product.AverageRating.ToString("0.0"),
                    TotalReviewCount = _reviewService.GetReviewCountForProduct(product.Id).ToString(),
                    IsDouble = (product.AverageRating % 1) == 0 ? false : true,
                    FloorAverageRating = (int)Math.Floor(product.AverageRating)
                },
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

        private List<string> SetProductImages(IEnumerable<ImageFile> imageFiles)
        {
            List<string> productImages = new List<string>();

            if (imageFiles.Count()>1)
            {
                foreach (var image in imageFiles)
                    productImages.Add(image.ImagePath);

                productImages.RemoveAt(0);
            }

            return productImages;
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
        #endregion
    }
}