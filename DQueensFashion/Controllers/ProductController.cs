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

        public ProductController(IProductService productService, ICustomerService customerService, IOrderService orderService, ICategoryService categoryService,
            IReviewService reviewService, IImageService imageService)
        {
            _productService = productService;
            _customerService = customerService;
            _orderService = orderService;
            _categoryService = categoryService;
            _reviewService = reviewService;
            _imageService = imageService;
        }
        // GET: Product
        public ActionResult Index(int categoryId=0)
        {
            var allImages = _imageService.GetAllImageFiles();

            IEnumerable<Product> _products = _productService.GetAllProducts().ToList();
           
            if (categoryId > 0)
                _products = _products.Where(p => p.Category.Id == categoryId);

            IEnumerable<ViewProductsViewModel> products = _products
                .Select(p => new ViewProductsViewModel()
                {
                    Id = p.Id,
                    Name = p.Name.Length > 17 ? p.Name.Substring(0, 15) + "..." : p.Name,
                    Description = p.Description.Length > 35 ? p.Description.Substring(0, 35) + "..." : p.Description,
                    MainImage = allImages.Where(image => image.ProductId == p.Id).Count() < 1 ?
                        AppConstant.DefaultProductImage :
                        allImages.Where(image => image.ProductId == p.Id).FirstOrDefault().ImagePath,
                    Quantity = p.Quantity,
                    Price = p.Price.ToString(),
                    Discount = p.Discount,
                    SubTotal = p.SubTotal.ToString(),
                    Category = p.Category.Name,
                    Rating = new RatingViewModel()
                    {
                        AverageRating = p.AverageRating.ToString("0.0"),
                        TotalReviewCount = _reviewService.GetReviewCountForProduct(p.Id).ToString(),
                        IsDouble = (p.AverageRating % 1) == 0 ? false : true,
                        FloorAverageRating = (int)Math.Floor(p.AverageRating)
                    },
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
                var allImages = _imageService.GetAllImageFiles();

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
                        Name = p.Name.Length > 17 ? p.Name.Substring(0, 15) + "..." : p.Name,
                        Description = p.Description.Length > 35 ? p.Description.Substring(0, 35) + "..." : p.Description,
                        MainImage = allImages.Where(image => image.ProductId == p.Id).Count() < 1 ?
                            AppConstant.DefaultProductImage :
                            allImages.Where(image => image.ProductId == p.Id).FirstOrDefault().ImagePath,
                        Category = p.Category.Name,
                        Quantity = p.Quantity,
                        Price = p.Price.ToString(),
                        Discount = p.Discount,
                        SubTotal = p.SubTotal.ToString(),
                        DateCreated = p.DateCreated,
                        Rating = new RatingViewModel()
                        {
                            AverageRating = p.AverageRating.ToString("0.0"),
                            TotalReviewCount = _reviewService.GetReviewCountForProduct(p.Id).ToString(),
                            IsDouble = (p.AverageRating % 1) == 0 ? false : true,
                            FloorAverageRating = (int)Math.Floor(p.AverageRating)
                        },
                        
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

                    case 8:
                        products = products.OrderByDescending(p => p.Rating.AverageRating);
                        break;

                    case 9:
                        products = products.OrderBy(p => p.Rating.AverageRating);
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
                var allImages = _imageService.GetAllImageFiles();

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
                        Name = p.Name.Length > 17 ? p.Name.Substring(0, 15) + "..." : p.Name,
                        Description = p.Description.Length > 35 ? p.Description.Substring(0, 35) + "..." : p.Description,
                        MainImage = allImages.Where(image => image.ProductId == p.Id).Count() < 1 ?
                            AppConstant.DefaultProductImage :
                            allImages.Where(image => image.ProductId == p.Id).FirstOrDefault().ImagePath,
                        Category = p.Category.Name,
                        Quantity = p.Quantity,
                        Price = p.Price.ToString(),
                        Discount = p.Discount,
                        SubTotal = p.SubTotal.ToString(),
                        DateCreated = p.DateCreated,
                        Rating = new RatingViewModel()
                        {
                            AverageRating = p.AverageRating.ToString("0.0"),
                            TotalReviewCount = _reviewService.GetReviewCountForProduct(p.Id).ToString(),
                            IsDouble = (p.AverageRating % 1) == 0 ? false : true,
                            FloorAverageRating = (int)Math.Floor(p.AverageRating)
                        },
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

                    case 8:
                        products = products.OrderByDescending(p => p.Rating.AverageRating);
                        break;

                    case 9:
                        products = products.OrderBy(p => p.Rating.AverageRating);
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

            var allProductImages = _imageService.GetImageFilesForProduct(product.Id);
            var productImages = SetProductImages(allProductImages);
        
            ViewProductsViewModel productDetails = new ViewProductsViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price.ToString(),
                Discount = product.Discount,
                SubTotal = product.SubTotal.ToString(),
                Quantity = product.Quantity,
                CategoryId = product.Category.Id,
                Category = product.Category.Name,
                Tags = product.Tags,
                DateCreatedString = product.DateCreated.ToString("dd/MMM/yyyy : hh-mm-ss"),
                MainImage = allProductImages.Count() < 1 ?
                        AppConstant.DefaultProductImage :
                        _imageService.GetMainImageForProduct(product.Id).ImagePath,
                OtherImagePaths = productImages,
                Rating = new RatingViewModel()
                {
                    AverageRating = product.AverageRating.ToString("0.0"),
                    TotalReviewCount = _reviewService.GetReviewCountForProduct(product.Id).ToString(),
                    IsDouble = (product.AverageRating % 1) == 0 ? false: true,
                    FloorAverageRating = (int) Math.Floor(product.AverageRating)
                },
                
                Reviews = _reviewService.GetAllReviewsForProduct(product.Id).ToList()
                    .Select(r => new ViewReviewViewModel() {
                        ReviewId = r.Id,
                        Name = r.Name,
                        Email = r.Email,
                        Comment = r.Comment,
                        Rating = r.Rating,
                        DateCreated = r.DateCreated.ToString("dd/MMM/yyyy"),
                        DateOrder = r.DateCreated,
                    }).OrderByDescending(r=>r.DateOrder).ToList(),
                WaistLength=product.WaistLength.HasValue?product.WaistLength.Value:false,
                ShoulderLength = product.ShoulderLength.HasValue?product.ShoulderLength.Value:false,
                BurstSize = product.BurstSize.HasValue?product.BurstSize.Value:false,
            };

            var allImages = _imageService.GetAllImageFiles();

            IEnumerable<ViewProductsViewModel> relatedProducts = _productService.GetRelatedProducts(product.Id, product.CategoryId)
                .Take(4).Select(p => new ViewProductsViewModel()
                {
                    Id = p.Id,
                    Name = p.Name.Length > 20 ? p.Name.Substring(0, 18) + "..." : p.Name,
                    Description = p.Description.Length > 35 ? p.Description.Substring(0, 35) + "..." : p.Description,
                    MainImage = allImages.Where(image => image.ProductId == p.Id).Count() < 1 ?
                        AppConstant.DefaultProductImage :
                        allImages.Where(image => image.ProductId == p.Id).FirstOrDefault().ImagePath,
                    Quantity = p.Quantity,
                    Price = p.Price.ToString(),
                    Discount = p.Discount,
                    SubTotal = p.SubTotal.ToString(),
                    Category = p.Category.Name,
                    Rating = new RatingViewModel()
                    {
                        AverageRating = p.AverageRating.ToString("0.0"),
                        TotalReviewCount = _reviewService.GetReviewCountForProduct(p.Id).ToString(),
                        IsDouble = (p.AverageRating % 1) == 0 ? false : true,
                        FloorAverageRating = (int)Math.Floor(p.AverageRating)
                    },
                }).ToList();

            ProductDetailsViewModel productModel = new ProductDetailsViewModel()
            {
                Product = productDetails,
                RelatedProducts = relatedProducts,
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
                       DateCreated = r.DateCreated.ToString("dd/MMM/yyyy"),
                       DateOrder = r.DateCreated,
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
                       DateCreated = r.DateCreated.ToString("dd/MMM/yyyy"),
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
                CustomerId = customer.Id,
                LineItems = lineItems,
                TotalAmount = lineItems.Sum(l => l.TotalAmount),
                TotalQuantity = lineItems.Sum(l => l.Quantity),
                OrderStatus=  OrderStatus.Processing,
            };

            _orderService.CreateOrder(order);
            return RedirectToAction("Index");
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
                Price = product.Price.ToString(),
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

        public ActionResult AddReview(int id=0)
        {
            Product product = _productService.GetProductById(id);
            if (product == null)
                return HttpNotFound();

            double averageRating = _reviewService.GetAverageRating(product.Id);

            AddReviewViewModel reviewModel = new AddReviewViewModel()
            {
                ProductId = product.Id,
                ProductName = product.Name.Length>20? product.Name.Substring(0,20) + "...":product.Name,
                //ProductImage = product.ImagePath1,
                ProductPrice = product.Price.ToString(),
                ProductSubTotal = product.Price.ToString(),
                ProductCategory = product.Category.Name,
                ProductAverageRating = new RatingViewModel()
                {
                    AverageRating = averageRating.ToString("0.0"),
                    TotalReviewCount = _reviewService.GetReviewCountForProduct(product.Id).ToString(),
                    IsDouble = (averageRating % 1) == 0 ? false : true,
                    FloorAverageRating = (int)Math.Floor(averageRating)
                },
            };

            return View(reviewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddReview(AddReviewViewModel reviewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "One or more validation errors");
                return View(reviewModel);
            }

            Product product = _productService.GetProductById(reviewModel.ProductId);
            if (product == null)
                throw new Exception();

            Review review = new Review()
            {
                Name = reviewModel.Name,
                Email = reviewModel.Email,
                Comment = reviewModel.Comment,
                Rating = reviewModel.Rating,
                Product = product,
            };

            _reviewService.AddReview(review);
            return RedirectToAction(nameof(ProductDetails),new { id = review.ProductId});
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