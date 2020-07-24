using DQueensFashion.Core.Model;
using DQueensFashion.CustomFilters;
using DQueensFashion.Models;
using DQueensFashion.Service.Contract;
using DQueensFashion.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DQueensFashion.Controllers
{
    [Authorize(Roles =AppConstant.AdminRole)]
    [AdminSetGlobalVariable]
    public class AdminController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IReviewService _reviewService;
        private readonly IImageService _imageService;
        private readonly ICustomerService _customerService;
        private readonly IGeneralValuesService _generalValuesService;
        private readonly IMailingListService _mailingListService;
        private readonly IMessageService _messageService;
        private readonly IRequestService _requestService;
        private ApplicationUserManager _userManager;
        private GeneralService generalService;

        public AdminController(ICategoryService categoryService, IProductService productService, IOrderService orderService, IReviewService reviewService, IImageService imageService, ICustomerService customerService, IGeneralValuesService generalValuesService, IMailingListService mailingListService,IRequestService requestService
            ,IMessageService messageService , ApplicationUserManager userManager)
        {
            _categoryService = categoryService;
            _productService = productService;
            _orderService = orderService;
            _reviewService = reviewService;
            _imageService = imageService;
            _customerService = customerService;
            _generalValuesService = generalValuesService;
            _mailingListService = mailingListService;
            _requestService = requestService;
            _messageService = messageService;
            _userManager = userManager;
            generalService = new GeneralService();
        }
        // GET: Admin
        public ActionResult Index()
        {
            return RedirectToAction(nameof(Dashboard));
        }

        public ActionResult Dashboard()
        {
            AdminViewModel adminModel = new AdminViewModel()
            {
                NumberOfCustomers = _customerService.GetAllCustomerCount(),
                NumberOfOrders = _orderService.GetAllOrders().Count(),
                NumberOfCategories = _categoryService.GetAllCategoriesCount(),
                NumberOfProducts = _productService.GetAllProductsCount(),
            };
            return View(adminModel);
        }

        public ActionResult Categories()
        {
            IEnumerable<ViewCategoryViewModel> categories = _categoryService.GetAllCategories()
                .Select(c => new ViewCategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                }).OrderBy(c => c.Name);
            return View(categories);
        }
        
        public ActionResult SearchCategories(string query)
        {
            IEnumerable<ViewCategoryViewModel> categories = _categoryService.GetAllCategories()
               .Select(c => new ViewCategoryViewModel()
               {
                   Id = c.Id,
                   Name = c.Name,
               }).OrderBy(c => c.Name);

            if (!string.IsNullOrEmpty(query))
                categories = categories.Where(c => c.Name.ToLower().Contains(query.ToLower())
                || string.Compare(c.Id.ToString(), query, true)==0
                );

            return PartialView("_categoriesTable",categories);
        }

        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCategory(AddCategoryViewModel categoryModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "One or more validation errors");
                return View();
            }

            try
            {

                if (_categoryService.GetCategoryByName(categoryModel.Name) != null)
                {
                    ModelState.AddModelError("", "Category already exists");
                    return View();
                }

                Category category = new Category()
                {
                    Name = categoryModel.Name,
                };
                _categoryService.AddCategory(category);

                return RedirectToAction(nameof(Categories));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult EditCategory(int id=0)
        {
            try
            {
                if (id == AppConstant.OutfitsId)
                    return RedirectToAction(nameof(Categories));

                Category category = _categoryService.GetCategoryById(id);
                if (category == null)
                    return HttpNotFound();

                EditCategoryViewModel categoryModel = new EditCategoryViewModel()
                {
                    Id = category.Id,
                    Name = category.Name,
                };
                TempData["categoryName"] = category.Name;
                return View(categoryModel);
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory(EditCategoryViewModel categoryModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "One or more validation errors");
                return View();
            }
            try
            {

                if (categoryModel.Id == AppConstant.OutfitsId)
                    return RedirectToAction(nameof(Categories));

                Category category = _categoryService.GetCategoryById(categoryModel.Id);
                if (category == null)
                    return HttpNotFound();

                category.Name = categoryModel.Name;
                _categoryService.UpdateCategory(category);

                return RedirectToAction(nameof(Categories));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult Products()
        {
            var allImages = _imageService.GetAllImageFiles().ToList();
            IEnumerable<ViewProductsViewModel> products = _productService.GetAllProducts()
                .Select(p => new ViewProductsViewModel()
                {
                    Id = p.Id,
                    GeneratedUrl = generalService.GenerateItemNameAsParam(p.Id, p.Name),
                    Name = p.Name,
                    Quantity = p.Quantity,
                    Price = p.Price,
                    Discount = p.Discount,
                    SubTotal = p.SubTotal,
                    Category = p.Category.Name,
                    MainImage = allImages.Where(image => image.ProductId == p.Id).Count() < 1 ?
                        AppConstant.DefaultProductImage :
                        allImages.Where(image => image.ProductId == p.Id).FirstOrDefault().ImagePath,
                    DateCreated = p.DateCreatedUtc,
                    DateCreatedString = generalService.GetDateInString(p.DateCreated,true,true),
                }).OrderByDescending(p=>p.DateCreated).ToList();
            return View(products);
        }

        public ActionResult SearchProducts(string query)
        {
            var allImages = _imageService.GetAllImageFiles().ToList();
            IEnumerable<ViewProductsViewModel> products = _productService.GetAllProducts()
              .Select(p => new ViewProductsViewModel()
              {
                  Id = p.Id,
                  GeneratedUrl = generalService.GenerateItemNameAsParam(p.Id, p.Name),
                  Name = p.Name,
                  Quantity = p.Quantity,
                  Price = p.Price,
                  Discount = p.Discount,
                  SubTotal = p.SubTotal,
                  Category = p.Category.Name,
                  MainImage = allImages.Where(image => image.ProductId == p.Id).Count() < 1 ?
                        AppConstant.DefaultProductImage :
                        allImages.Where(image => image.ProductId == p.Id).FirstOrDefault().ImagePath,
                  DateCreated = p.DateCreatedUtc,
                  DateCreatedString = generalService.GetDateInString(p.DateCreated, true, true),
              }).OrderByDescending(p => p.DateCreated).ToList();

            if (!string.IsNullOrEmpty(query))
                products = products.Where(p => p.Name.ToLower().Contains(query.ToLower())
                || p.Category.ToLower().Contains(query.ToLower())
                || string.Compare(p.Quantity.ToString(), query, true) == 0
                || p.Price.ToString().Contains(query)
                );

            return PartialView("_productsTable",products);
        }

        public ActionResult AddProduct(int categoryId=0)
        {
            IEnumerable<CategoryNameAndId> categories = _categoryService.GetAllCategories()
                .Select(c => new CategoryNameAndId()
                {
                    Id = c.Id,
                    Name = c.Name,
                }).OrderBy(c=>c.Name).ToList();

            ProductViewModel productModel = new ProductViewModel()
            {
                Categories = categories,
                Quantity = 1,
                DeliveryDaysDuration = 1,
            };

            if (categoryId == AppConstant.OutfitsId)
                productModel.OutfitCategory = true;
           
            return View(productModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProduct(ProductViewModel productModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "One or more validation errors");

                IEnumerable<CategoryNameAndId> categories = _categoryService.GetAllCategories()
                   .Select(c => new CategoryNameAndId()
                   {
                       Id = c.Id,
                       Name = c.Name,
                   }).OrderBy(c => c.Name).ToList();

                productModel.Categories = categories;

                return View(productModel);
            }

            Category category = _categoryService.GetCategoryById(productModel.CategoryId);
            if (category == null)
                throw new Exception();

            Product product = new Product()
            {
                Name = productModel.Name,
                Description = productModel.Description,
                Quantity = productModel.Quantity,
                Price = Math.Round(productModel.Price, 2,MidpointRounding.AwayFromZero),
                Discount = Math.Round(productModel.Discount, 2,MidpointRounding.AwayFromZero),
                SubTotal = _productService.CalculateProductPrice(productModel.Price, productModel.Discount),
                Category = category,
                Tags = productModel.Tags != null ? String.Join(",", productModel.Tags) : "",
                DeliveryDaysDuration = productModel.DeliveryDaysDuration,

                //measurement
                Shoulder = productModel.Shoulder,
                ArmHole = productModel.ArmHole,
                Bust = productModel.Bust,
                Waist= productModel.Waist,
                Hips = productModel.Hips,
                Thigh = productModel.Thigh,
                FullBodyLength = productModel.FullBodyLength,
                KneeGarmentLength = productModel.KneeGarmentLength,
                TopLength = productModel.TopLength,
                TrousersLength = productModel.TrousersLength,
                RoundAnkle = productModel.RoundAnkle,
                NipNip = productModel.NipNip,
                SleeveLength = productModel.SleeveLength,

            };

            _productService.AddProduct(product);
            ViewBag.DefaultImage = AppConstant.DefaultProductImage;
            return RedirectToAction(nameof(AddProductImages),new { id=product.Id});
        }

        public ActionResult AddProductImages(int id=0)
        {
            Product product = _productService.GetProductById(id);
            if (product == null)
                return HttpNotFound();

            AddProductImageViewModel productImageModel = new AddProductImageViewModel()
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductCategory = product.Category.Name,
            };

            return View(productImageModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProductImages(AddProductImageViewModel productImageModel)
        {
            var imageFiles = Request.Files;
            if (!FileService.ValidateProductImages(imageFiles))
                throw new Exception();
            
            Product product = _productService.GetProductById(productImageModel.ProductId);
            if (product == null)
                throw new Exception();

            List<ImageFile> ImageModel = new List<ImageFile>();

            for (int i = 0; i < imageFiles.Count; i++)
            {
                if (imageFiles[i]!= null && imageFiles[i].ContentLength > 0)
                {
                    string fileName = FileService.GetFileName(imageFiles[i]);
                    string imgPath = AppConstant.ProductImageBasePath + fileName;
                    fileName = Path.Combine(Server.MapPath(AppConstant.ProductImageBasePath), fileName);
                    FileService.SaveImage(imageFiles[i], fileName);
                    ImageModel.Add(new ImageFile { ProductId = product.Id, ImagePath = imgPath }); 
                }
            }

            _imageService.AddRangeImages(ImageModel);
            return RedirectToAction(nameof(ProductDetails), new { id = product.Id });
        }

        public ActionResult EditProduct(int id=0)
        {
            Product product = _productService.GetProductById(id);
            if (product == null)
                return HttpNotFound();
            var allCategories = _categoryService.GetAllCategories().ToList();

            ProductViewModel productModel = new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Quantity = product.Quantity,
                Price = product.Price,
                Discount = product.Discount,
                SubTotal = product.SubTotal,
                Categories = allCategories.Select(c => new CategoryNameAndId()
                {
                    Id = c.Id,
                    Name = c.Name,
                }).OrderBy(c => c.Name).ToList(),
                Tags = string.IsNullOrEmpty(product.Tags) ? new List<string>() : product.Tags.Split(',').ToList(),
                DeliveryDaysDuration = product.DeliveryDaysDuration,

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

                OutfitCategory = product.CategoryId == AppConstant.OutfitsId ? true : false,
            };

            //foreach (var category in productModel.Categories)
            //{
            //    if (category.Id == product.CategoryId)
            //        category.Selected = "selected";
            //}

            return View(productModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProduct(ProductViewModel productModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "One or more validation errors");
                Category _category = _categoryService.GetCategoryById(productModel.CategoryId);
                if (_category == null)
                    throw new Exception();

                Product _product = _productService.GetProductById(productModel.Id);
                if (_product == null)
                    throw new Exception();

                productModel.Categories = _categoryService.GetAllCategories()
                    .Select(c => new CategoryNameAndId() {
                        Id = c.Id,
                        Name = c.Name,
                    }).OrderBy(c => c.Name).ToList();

                productModel.Tags = productModel.Tags == null ? new List<string>() : productModel.Tags;

                //foreach(var c in productModel.Categories)
                //{
                //    if (c.Id == productModel.CategoryId)
                //        c.Selected = "selected";
                //}

                return View(productModel);
            }

            Product product = _productService.GetProductById(productModel.Id);
            if (product == null)
                throw new Exception();

            Category category = _categoryService.GetCategoryById(productModel.CategoryId);
            if (category == null)
                throw new Exception();

            product.Name = productModel.Name;
            product.Description = productModel.Description;
            product.Quantity = productModel.Quantity;
            product.Price = Math.Round(productModel.Price,2,MidpointRounding.AwayFromZero);
            product.Discount = Math.Round(productModel.Discount, 2,MidpointRounding.AwayFromZero);
            product.SubTotal = _productService.CalculateProductPrice(productModel.Price, productModel.Discount);
            product.Category = category;
            product.Tags = productModel.Tags != null ? String.Join(",", productModel.Tags) : "";
            product.DeliveryDaysDuration = productModel.DeliveryDaysDuration;

            //measurements
            product.Shoulder = productModel.Shoulder;
            product.ArmHole = productModel.ArmHole;
            product.Bust = productModel.Bust;
            product.Waist = productModel.Waist;
            product.Hips = productModel.Hips;
            product.Thigh = productModel.Thigh;
            product.FullBodyLength = productModel.FullBodyLength;
            product.KneeGarmentLength = productModel.KneeGarmentLength;
            product.TopLength = productModel.TopLength;
            product.TrousersLength = productModel.TrousersLength;
            product.RoundAnkle = productModel.RoundAnkle;
            product.NipNip = productModel.NipNip;
            product.SleeveLength = productModel.SleeveLength;

            _productService.UpdateProduct(product);
            return RedirectToAction(nameof(Products));
        }

        public ActionResult ProductDetails(int id=0)
        {
            Product product = _productService.GetProductById(id);
            if (product == null)
                return HttpNotFound();

            var allProductImages = _imageService.GetImageFilesForProduct(product.Id);
            var productImages = SetProductImages(allProductImages);

            double averageRating = _reviewService.GetAverageRating(product.Id);

            ViewProductsViewModel productDetails = new ViewProductsViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Discount = product.Discount,
                SubTotal = product.SubTotal,
                Quantity = product.Quantity,
                Category = product.Category.Name,
                DeliveryDaysDuration = product.DeliveryDaysDuration,
                Tags = product.Tags,
                DateCreatedString = generalService.GetDateInString(product.DateCreated, true, true),
                MainImage = allProductImages.Count() < 1 ?
                        AppConstant.DefaultProductImage :
                        _imageService.GetMainImageForProduct(product.Id).ImagePath,
                OtherImagePaths = productImages,
                Rating = new RatingViewModel()
                {
                    AverageRating = averageRating.ToString("0.0"),
                    TotalReviewCount = _reviewService.GetReviewCountForProduct(product.Id).ToString(),
                    IsDouble = (averageRating % 1) == 0 ? false : true,
                    FloorAverageRating = (int)Math.Floor(averageRating)
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
            };

            ProductDetailsViewModel productModel = new ProductDetailsViewModel()
            {
                Product = productDetails,
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

        public ActionResult EditProductImages(int id = 0)
        {
            Product product = _productService.GetProductById(id);
            if (product == null)
                return HttpNotFound();

            EditProductImageViewModel imageModels = new EditProductImageViewModel()
            {
                ProductId = product.Id,
                ProductCategory = product.Category.Name,
                ProductName = product.Name,
                ProductImages = _imageService.GetImageFilesForProduct(product.Id)
                    .Select(image=> new ImageViewModel() { ImagePath= image.ImagePath, Id=image.Id}),
            };

            return View(imageModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProductImages(EditProductImageViewModel productImageModel)
        {
            var imageFiles = Request.Files;
            if (!FileService.ValidateProductImages(imageFiles))
                throw new Exception();

            Product product = _productService.GetProductById(productImageModel.ProductId);
            if (product == null)
                throw new Exception();

            List<ImageFile> ImageModel = new List<ImageFile>();

            for (int i = 0; i < imageFiles.Count; i++)
            {
                if (imageFiles[i] != null && imageFiles[i].ContentLength > 0)
                {
                    string fileName = FileService.GetFileName(imageFiles[i]);
                    string imgPath = AppConstant.ProductImageBasePath + fileName;
                    fileName = Path.Combine(Server.MapPath(AppConstant.ProductImageBasePath), fileName);
                    FileService.SaveImage(imageFiles[i], fileName);
                    ImageModel.Add(new ImageFile { ProductId = product.Id, ImagePath = imgPath });
                }
            }

            _imageService.AddRangeImages(ImageModel);
            return RedirectToAction(nameof(ProductDetails),new { id=product.Id});
        }

        public PartialViewResult DeleteProductImage(int id)
        {
            ImageFile imageFile = _imageService.GetImageById(id);
            if (imageFile == null)
                throw new Exception();
          
            _imageService.DeleteImage(imageFile);

            try
            {
                string fullPath = Request.MapPath(imageFile.ImagePath);
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            catch (Exception)
            {

            }

            IEnumerable<ImageViewModel> images = _imageService.GetImageFilesForProduct(imageFile.ProductId)
                .Select(image => new ImageViewModel()
                {
                    Id=image.Id,
                    ImagePath = image.ImagePath,
                });
            return PartialView("_productImages",images);
        }

        public ActionResult OrderDetails(int id=0)
        {
            Order order = _orderService.GetOrderById(id);
            if (order == null)
                return HttpNotFound();

            var allImages = _imageService.GetAllImageFiles();

            ViewOrderViewModel orderModel = new ViewOrderViewModel()
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                CustomerName = order.FirstName + " " + order.LastName,
                CustomerPhone = order.Phone,
                CustomerAddress = order.Address,
                SubTotal = order.SubTotal,
                ShippingPrice = order.ShippingPrice,
                TotalAmount = order.TotalAmount,
                TotalQuantity = order.TotalQuantity,
                LineItems = order.LineItems
                        .Select(lineItem => new ViewLineItem()
                        {
                            ProductName = lineItem.Product.Name,
                            Quantity = lineItem.Quantity,
                            UnitPrice = lineItem.UnitPrice,
                            TotalAmount = lineItem.TotalAmount,
                            ProductImage = allImages.Where(image => image.ProductId == lineItem.Product.Id).Count() < 1 ?
                                AppConstant.DefaultProductImage :
                                allImages.Where(image => image.ProductId == lineItem.Product.Id).FirstOrDefault().ImagePath,
                            Description = string.IsNullOrEmpty(lineItem.Description) ? "" : lineItem.Description.Replace("\r\n", " , "),
                            IsOutfit = lineItem.Product.CategoryId == AppConstant.OutfitsId ? true : false,
                        }),
                OrderStatus = order.OrderStatus.ToString(),
                DateCreated = order.DateCreatedUtc,

                DateCreatedString = generalService.GetDateInString(order.DateCreated, true, false),
                LineItemConcatenatedString = string.Join(",", order.LineItems.Select(x => x.Product.Name)),

            };


            return View(orderModel);
        }

        public ActionResult Orders(string query="")
        {
            IEnumerable<ViewOrderViewModel> orderModel = _orderService.GetAllOrders()
                .Select(order => new ViewOrderViewModel()
                {
                    OrderId = order.Id,
                    CustomerId = order.CustomerId,
                    CustomerName = order.FirstName + " "+ order.LastName,
                    TotalAmount = order.TotalAmount,
                    TotalQuantity = order.TotalQuantity,
                    LineItems = order.LineItems
                        .Select(lineItem => new ViewLineItem()
                        {
                            ProductName = lineItem.Product.Name,
                            Quantity = lineItem.Quantity,
                            TotalAmount = lineItem.TotalAmount,
                        }),
                    OrderStatus= order.OrderStatus.ToString(),
                    DateCreated = order.DateCreatedUtc,
                    DateCreatedString = generalService.GetDateInString(order.DateCreated,false,false),
                    LineItemConcatenatedString = string.Join(",", order.LineItems.Select(x => x.Product.Name)),
                }).OrderByDescending(order=>order.DateCreated).ToList();

            if (!string.IsNullOrEmpty(query))
                orderModel = orderModel.Where(order => order.CustomerName.ToLower().Contains(query.ToLower())
                || order.LineItemConcatenatedString.ToLower().Contains(query.ToLower())
                || order.OrderStatus.ToString().ToLower().Contains(query.ToLower())
                || (string.Compare(order.OrderId.ToString(), query, true) == 0)
                ).ToList();

            ViewBag.Query = query;

            return View(orderModel);
        }

        public ActionResult ProcessingOrders(string query="")
        {
            IEnumerable<ViewOrderViewModel> orderModel = _orderService.GetProcessingOrders()
                .Select(order => new ViewOrderViewModel()
                {
                    OrderId = order.Id,
                    CustomerId = order.CustomerId,
                    CustomerName = order.FirstName + " " + order.LastName,
                    TotalAmount = order.TotalAmount,
                    TotalQuantity = order.TotalQuantity,
                    LineItems = order.LineItems
                        .Select(lineItem => new ViewLineItem()
                        {
                            ProductName = lineItem.Product.Name,
                            Quantity = lineItem.Quantity,
                            TotalAmount = lineItem.TotalAmount,
                        }),
                    DateCreated = order.DateCreatedUtc,
                    DateCreatedString = generalService.GetDateInString(order.DateCreated, false, false),
                    DateModified = order.DateModified,
                    LineItemConcatenatedString = string.Join(",", order.LineItems.Select(x => x.Product.Name)),
                }).OrderByDescending(order => order.DateModified).ToList();

            if (!string.IsNullOrEmpty(query))
                orderModel = orderModel.Where(order => order.CustomerName.ToLower().Contains(query.ToLower())
                || order.LineItemConcatenatedString.ToLower().Contains(query.ToLower())
                || (string.Compare(order.OrderId.ToString(), query, true) == 0)
                ).ToList();

            ViewBag.Query = query;
            return View(orderModel);
        }

        public ActionResult InTransitOrders(string query="")
        {
            IEnumerable<ViewOrderViewModel> orderModel = _orderService.GetInTransitOrders()
               .Select(order => new ViewOrderViewModel()
               {
                   OrderId = order.Id,
                   CustomerId = order.CustomerId,
                   CustomerName = order.FirstName + " " + order.LastName,
                   TotalAmount = order.TotalAmount,
                   TotalQuantity = order.TotalQuantity,
                   LineItems = order.LineItems
                       .Select(lineItem => new ViewLineItem()
                       {
                           ProductName = lineItem.Product.Name,
                           Quantity = lineItem.Quantity,
                           TotalAmount = lineItem.TotalAmount,
                       }),
                   DateCreated = order.DateCreatedUtc,
                   DateCreatedString = generalService.GetDateInString(order.DateCreated, false, false),
                   LineItemConcatenatedString = string.Join(",", order.LineItems.Select(x => x.Product.Name)),
                   DateModified = order.DateModified,
               }).OrderByDescending(order => order.DateModified).ToList();

            if (!string.IsNullOrEmpty(query))
                orderModel = orderModel.Where(order => order.CustomerName.ToLower().Contains(query.ToLower())
                || order.LineItemConcatenatedString.ToLower().Contains(query.ToLower())
                || (string.Compare(order.OrderId.ToString(), query, true) == 0)
                ).ToList();

            ViewBag.Query = query;
            return View(orderModel);
        }

        public ActionResult DeliveredOrders(string query="")
        {
            IEnumerable<ViewOrderViewModel> orderModel = _orderService.GetDeliveredOrders()
                .Select(order => new ViewOrderViewModel()
                {
                    OrderId = order.Id,
                    CustomerId = order.CustomerId,
                    CustomerName = order.FirstName + " " + order.LastName,
                    TotalAmount = order.TotalAmount,
                    TotalQuantity = order.TotalQuantity,
                    LineItems = order.LineItems
                        .Select(lineItem => new ViewLineItem()
                        {
                            ProductName = lineItem.Product.Name,
                            Quantity = lineItem.Quantity,
                            TotalAmount = lineItem.TotalAmount,
                        }),
                    DateCreated = order.DateCreatedUtc,
                    DateCreatedString = generalService.GetDateInString(order.DateCreated, false, false),
                    LineItemConcatenatedString = string.Join(",", order.LineItems.Select(x => x.Product.Name)),
                    DateModified = order.DateModified,
                }).OrderByDescending(order => order.DateModified).ToList();

            if (!string.IsNullOrEmpty(query))
                orderModel = orderModel.Where(order => order.CustomerName.ToLower().Contains(query.ToLower())
                || order.LineItemConcatenatedString.ToLower().Contains(query.ToLower())
                || (string.Compare(order.OrderId.ToString(), query, true) == 0)
                ).ToList();

            ViewBag.Query = query;
            return View(orderModel);
        }

        public ActionResult ReturnedOrders(string query="")
        {
            IEnumerable<ViewOrderViewModel> orderModel = _orderService.GetReturnedOrders()
                .Select(order => new ViewOrderViewModel()
                {
                    OrderId = order.Id,
                    CustomerId = order.CustomerId,
                    CustomerName = order.FirstName + " " + order.LastName,
                    TotalAmount = order.TotalAmount,
                    TotalQuantity = order.TotalQuantity,
                    LineItems = order.LineItems
                        .Select(lineItem => new ViewLineItem()
                        {
                            ProductName = lineItem.Product.Name,
                            Quantity = lineItem.Quantity,
                            TotalAmount = lineItem.TotalAmount,
                        }),
                    DateCreated = order.DateCreatedUtc,
                    DateCreatedString = generalService.GetDateInString(order.DateCreated, false, false),
                    LineItemConcatenatedString = string.Join(",", order.LineItems.Select(x => x.Product.Name)),
                    DateModified = order.DateModified,
                }).OrderByDescending(order => order.DateModified).ToList();

            if (!string.IsNullOrEmpty(query))
                orderModel = orderModel.Where(order => order.CustomerName.ToLower().Contains(query.ToLower())
                || order.LineItemConcatenatedString.ToLower().Contains(query.ToLower())
                || (string.Compare(order.OrderId.ToString(), query, true) == 0)
                ).ToList();

            ViewBag.Query = query;
            return View(orderModel);
        }

        public ActionResult CompletedOrders(string query="")
        {
            IEnumerable<ViewOrderViewModel> orderModel = _orderService.GetCompletedOrders()
              .Select(order => new ViewOrderViewModel()
              {
                  OrderId = order.Id,
                  CustomerId = order.CustomerId,
                  CustomerName = order.FirstName + " " + order.LastName,
                  TotalAmount = order.TotalAmount,
                  TotalQuantity = order.TotalQuantity,
                  LineItems = order.LineItems
                      .Select(lineItem => new ViewLineItem()
                      {
                          ProductName = lineItem.Product.Name,
                          Quantity = lineItem.Quantity,
                          TotalAmount = lineItem.TotalAmount,
                      }),
                  DateCreated = order.DateCreatedUtc,
                  DateCreatedString = generalService.GetDateInString(order.DateCreated, false, false),
                  LineItemConcatenatedString = string.Join(",", order.LineItems.Select(x => x.Product.Name)),
                  DateModified = order.DateModified,
              }).OrderByDescending(order => order.DateModified).ToList();

            if (!string.IsNullOrEmpty(query))
                orderModel = orderModel.Where(order => order.CustomerName.ToLower().Contains(query.ToLower())
                || order.LineItemConcatenatedString.ToLower().Contains(query.ToLower())
                || (string.Compare(order.OrderId.ToString(), query, true) == 0)
                ).ToList();

            ViewBag.Query = query;
            return View(orderModel);
        }

        public ActionResult TransitOrder(int id)
        {
            Order order = _orderService.GetOrderById(id);
            if (order == null)
                throw new Exception();

            order.OrderStatus = OrderStatus.InTransit;
            _orderService.UpdateOrder(order);

            return RedirectToAction(nameof(InTransitOrders));
        }

        public ActionResult DeliverOrder(int id)
        {
            Order order = _orderService.GetOrderById(id);
            if (order == null)
                throw new Exception();

            order.OrderStatus = OrderStatus.Delivered;
            _orderService.UpdateOrder(order);

            return RedirectToAction(nameof(DeliveredOrders));
        }

        public ActionResult ReturnOrder(int id)
        {
            Order order = _orderService.GetOrderById(id);
            if (order == null)
                throw new Exception();

            order.OrderStatus = OrderStatus.Returned;
            _orderService.UpdateOrder(order);

            return RedirectToAction(nameof(ReturnedOrders));
        }

        public ActionResult CompleteOrder(int id)
        {
            Order order = _orderService.GetOrderById(id);
            if (order == null)
                throw new Exception();

            order.OrderStatus = OrderStatus.Completed;
            _orderService.UpdateOrder(order);

            return RedirectToAction(nameof(CompletedOrders));
        }

        public ActionResult UpdateOrderStatus(int id)
        {
            try
            {
                Order order = _orderService.GetOrderById(id);
                if (order == null)
                    throw new Exception();

                UpdateOrderStatusViewModel orderModel = new UpdateOrderStatusViewModel()
                {
                    Id=order.Id,
                    OrderStatus = order.OrderStatus,
                };

                return PartialView("_updateOrderStatus",orderModel);
            }
            catch (Exception ex)
            {
                var a = ex;
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateOrderStatus(UpdateOrderStatusViewModel orderModel)
        {
            Order order = _orderService.GetOrderById(orderModel.Id);
            if (order == null)
                throw new Exception();

            order.OrderStatus = orderModel.OrderStatus;
            _orderService.UpdateOrder(order);

            return RedirectToAction(nameof(Orders));
        }

        public ActionResult Customers()
        {
            IEnumerable<CustomerViewModel> allCustomers = _customerService.GetAllCustomers()
                .Select(c => new CustomerViewModel
                {
                    CustomerId = c.Id,
                    CustomerEmail = c.Email,
                    TotalCustomerOrders = _orderService.GetAllOrdersForCustomer(c.Id).Count()
                }).ToList();

            return View(allCustomers);
        }

        public ActionResult SearchCustomers(string query)
        {
            IEnumerable<CustomerViewModel> allCustomers = _customerService.GetAllCustomers()
               .Select(c => new CustomerViewModel
               {
                   CustomerId = c.Id,
                   CustomerEmail = c.Email,
                   TotalCustomerOrders = _orderService.GetAllOrdersForCustomer(c.Id).Count()
               }).ToList();

            if (!string.IsNullOrEmpty(query))
                allCustomers = allCustomers.Where(c => c.CustomerEmail.ToLower().Contains(query.ToLower()));

            return PartialView("_customersTable", allCustomers);

        }

        public ActionResult CustomerOrders(int id)
        {
            Customer customer = _customerService.GetCustomerById(id);
            if (customer == null)
                return HttpNotFound();

            IEnumerable<ViewOrderViewModel> orderModel = _orderService.GetAllOrdersForCustomer(customer.Id)
                 .Select(order => new ViewOrderViewModel()
                 {
                     OrderId = order.Id,
                     CustomerId = order.CustomerId,
                     TotalAmount = order.TotalAmount,
                     TotalQuantity = order.TotalQuantity,
                     LineItems = order.LineItems
                         .Select(lineItem => new ViewLineItem()
                         {
                             ProductName = lineItem.Product.Name,
                             Quantity = lineItem.Quantity,
                             TotalAmount = lineItem.TotalAmount,
                         }),
                     OrderStatus = order.OrderStatus.ToString(),
                     DateCreated = order.DateCreatedUtc,
                     DateCreatedString = generalService.GetDateInString(order.DateCreated, false, false),
                 }).OrderByDescending(order => order.DateCreated).ToList();

            return View(orderModel);
        }

        public ActionResult GeneralDetails(string success="")
        {
            GeneralValues generalValues = _generalValuesService.GetGeneralValues();
            GeneralValuesViewModel generalValuesModel = new GeneralValuesViewModel()
            {
                GeneralValId = generalValues.Id,
                NewsLetterSubscriptionDiscount = generalValues.NewsLetterSubscriptionDiscount,
                ShippingPrice = generalValues.ShippingPrice,
            };

            ViewBag.Success = success;
            return View(generalValuesModel);
                
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GeneralDetails(GeneralValuesViewModel generalValuesModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "One or more validation errors");
                return View(generalValuesModel);
            }

            if (generalValuesModel.GeneralValId != AppConstant.GeneralValId)
                throw new Exception();

            GeneralValues generalValues = _generalValuesService.GetGeneralValues();
            generalValues.NewsLetterSubscriptionDiscount = generalValuesModel.NewsLetterSubscriptionDiscount;
            generalValues.ShippingPrice = generalValuesModel.ShippingPrice;

            _generalValuesService.UpdateGeneralValues(generalValues);
            return RedirectToAction(nameof(GeneralDetails), new { success = "success" });
        }

        public ActionResult SubscribedEmails()
        {
            var allEmails = _mailingListService.GetAllMailingList();
            IEnumerable<EmailsViewModel> emailList = allEmails
                .Select(em => new EmailsViewModel()
                {
                    Id = em.Id,
                    Email = em.EmailAddress,
                    IsSelected = true,
                }).OrderBy(e => e.Email).ToList();

            string emailListCopy = string.Empty;
            foreach(var email in allEmails)
            {
                emailListCopy += email.EmailAddress.ToString() + " ";
            }

            ViewBag.EmailListCopy = emailListCopy;

            return View(emailList);
        }

        public ActionResult CreateNewsLetter()
        {
            IEnumerable<EmailsViewModel> emailList = _mailingListService.GetAllMailingList()
                .Select(em => new EmailsViewModel()
                {
                    Id = em.Id,
                    Email = em.EmailAddress,
                    IsSelected = true,
                }).OrderBy(e=>e.Email).ToList();

            CreateNewsLetterViewModel newsLetterModel = new CreateNewsLetterViewModel()
            {
                EmailList = emailList.ToList(),
            };

            return View(newsLetterModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateNewsLetter(CreateNewsLetterViewModel newsLetterModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "One or more validation errors");
                throw new Exception();
            }

            List<string> emails = new List<string>();
            foreach(var email in newsLetterModel.EmailList)
            {
                if (email.IsSelected)
                    emails.Add(email.Email);
            }

            MailService mailService = new MailService();
            var credentials = AppConstant.HDQ_INFO_ACCOUNT_MAIL_CREDENTIALS;
            await mailService.SendMailToMultiple(AppConstant.HDQ_MESSAGE_MAIL_ACCOUNT, newsLetterModel.Title, newsLetterModel.Message,credentials,emails,
                AppConstant.HDQ_INFO_MAIL_ACCOUNT,"HDQ Newsletter");

            return RedirectToAction(nameof(SubscribedEmails));
        }

        public ActionResult Messages()
        {
            IEnumerable<MessageViewModel> messages = _messageService.GetMessages()
                .Select(m => new MessageViewModel()
                {
                    Id= m.Id,
                    Email = m.Email.Length > 20 ? m.Email.Substring(0, 17) + "..." : m.Email,
                    Fullname = m.Fullname.Length > 20 ? m.Fullname.Substring(0, 17) + "..." : m.Fullname,
                    Phone = m.Phone.Length > 20 ? m.Phone.Substring(0, 17) + "..." : m.Phone,
                    MessageSummary = m.MessageSummary.Length > 20 ? m.MessageSummary.Substring(0, 17) + "..." : m.MessageSummary,
                    DateCreated = m.DateCreatedUtc,
                    Read = m.Read,
                }).OrderByDescending(m => m.DateCreated).ToList();

            return View(messages);
        }

        public ActionResult Message(int id=0)
        {
            DQueensFashion.Core.Model.Message message = _messageService.GetMessageById(id);
            if (message == null)
                return HttpNotFound();

            message.Read = true;
            _messageService.UpdateMessage(message);

            MessageViewModel messageModel = new MessageViewModel()
            {
                Fullname = message.Fullname,
                Email = message.Email,
                Phone = message.Phone,
                Subject = message.Subject,
                MessageSummary = message.MessageSummary,
                DateCreated = message.DateCreated,
            };

            return View(messageModel);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel passwordModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "One or more validation errors");
                return View(passwordModel);
            }

            var userId = GetLoggedInUserId();
            var result = await _userManager.ChangePasswordAsync(userId, passwordModel.OldPassword, passwordModel.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Dashboard));
            }
            else
            {
                throw new Exception();
            }
        }

        public ActionResult Requests()
        {
            var allImages = _imageService.GetAllImageFiles().ToList();
            var _reqGroup = (from r in _requestService.GetAllRequests()
                             group r by r.ProductId into g
                        select new { request = g.Key,
                            users = g.OrderBy(rr => rr.CustomerEmail).ToList() }).ToList();

            IEnumerable<ViewRequestsViewModel> requestsGroup = _reqGroup
                .Select(r => new ViewRequestsViewModel()
                {
                    Product = _productService.GetProductById(r.request),
                    ProductName = _productService.GetProductById(r.request).Name,
                    MainImage = allImages.Where(image => image.ProductId == r.request).Count() < 1 ?
                                AppConstant.DefaultProductImage :
                                allImages.Where(image => image.ProductId == r.request).FirstOrDefault().ImagePath,
                    UsersRequests = r.users
                        .Select(user => new RequestViewModel()
                        {
                            CustomerEmail = user.CustomerEmail,
                            Quantity = user.Quantity,
                        }).ToList(),
                }).OrderBy(r=>r.ProductName).ToList();

            return View(requestsGroup);
        }

        public ActionResult RequestReply(int id=0)
        {
            Product product = _productService.GetProductById(id);
            if (product == null)
                throw new Exception();

            var requests = _requestService.GetAllRequestsForProduct(id);
            if (requests.Count() < 1)
                return RedirectToAction(nameof(Request));
            List<string> requestEmails = requests.Select(c => c.CustomerEmail).ToList();
            requestEmails = requestEmails.Distinct().ToList();

            var allProductImages = _imageService.GetImageFilesForProduct(product.Id);
            var productImage = _imageService.GetMainImageForProduct(product.Id).ImagePath;

            IEnumerable<EmailsViewModel> emailList = requestEmails
              .Select(em => new EmailsViewModel()
              {
                  Email = em,
                  IsSelected = true,
              }).OrderBy(e => e.Email).ToList();

            RequestsReplyViewModel requestModel = new RequestsReplyViewModel()
            {
                EmailList = emailList.ToList(),
                Product = product,
                ProductId = id,
                Message = "Product link: https://houseofdqueens.com/Product/ProductDetails/" + product.Name + "-" + id.ToString(),
                MainImage = string.IsNullOrEmpty(productImage)?
                        AppConstant.DefaultProductImage :
                        productImage
            };

            return View(requestModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RequestReply(RequestsReplyViewModel requestModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("", "One or more validation errors");
                    return View(requestModel);
                }

                List<string> emails = new List<string>();
                foreach (var email in requestModel.EmailList)
                {
                    if (email.IsSelected)
                        emails.Add(email.Email);
                }

                MailService mailService = new MailService();
                var credentials = AppConstant.HDQ_INFO_ACCOUNT_MAIL_CREDENTIALS;
                await mailService.SendMailToMultiple(AppConstant.HDQ_MESSAGE_MAIL_ACCOUNT, requestModel.Title, requestModel.Message, credentials, emails,
                    AppConstant.HDQ_INFO_MAIL_ACCOUNT, "HDQ Request Available");

                var requests = _requestService.GetAllRequests().Where(r => r.ProductId == requestModel.ProductId);
                _requestService.DeleteRequestsRange(requests);
                return RedirectToAction(nameof(requests));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult DeleteProduct(int id)
        {
            Product product = _productService.GetProductById(id);
            if (product == null)
                return HttpNotFound();

            string mainImage = _imageService.GetImageFilesForProduct(product.Id).Count() < 1
             ? AppConstant.DefaultProductImage
             : _imageService.GetMainImageForProduct(product.Id).ImagePath;

            DeleteProductViewModel productViewModel = new DeleteProductViewModel()
            {
                ProductName = product.Name,
                ProductId = product.Id,
                CategoryName = product.Category.Name,
                ProductImage = mainImage,
            };

            return PartialView("_deleteProduct",productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteProduct(DeleteProductViewModel productModel)
        {
            var user = _userManager.FindById(GetLoggedInUserId());
            if (!_userManager.CheckPassword(user, productModel.AdminPassword))
                throw new Exception();

            Product product = _productService.GetProductById(productModel.ProductId);
            if (product == null)
                throw new Exception();

            IEnumerable<ImageFile> productImages = _imageService.GetImageFilesForProduct(product.Id);
            foreach(var imageFile in productImages)
            {
                _imageService.DeleteImage(imageFile);
                try
                {
                    string fullPath = Request.MapPath(imageFile.ImagePath);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                }
                catch (Exception)
                {

                }
            }

            _productService.DeleteProduct(product);
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteCategory(int id)
        {
            Category category = _categoryService.GetCategoryById(id);
            if (category == null)
                return HttpNotFound();

            DeleteCategoryViewModel categoryModel = new DeleteCategoryViewModel()
            {
                CategoryId = category.Id,
                CategoryName = category.Name,
            };
            return PartialView("_deleteCategory", categoryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCategory(DeleteCategoryViewModel categoryModel)
    {
            var user = _userManager.FindById(GetLoggedInUserId());
            if (!_userManager.CheckPassword(user, categoryModel.AdminPassword))
                throw new Exception();

            Category category = _categoryService.GetCategoryById(categoryModel.CategoryId);
            if (category == null)
                throw new Exception();

            var allCategoryProducts = _productService.GetAllProducts()
                .Where(p => p.CategoryId == categoryModel.CategoryId);

            foreach(var product in allCategoryProducts)
            {
                IEnumerable<ImageFile> productImages = _imageService.GetImageFilesForProduct(product.Id);
                foreach (var imageFile in productImages)
                {
                    _imageService.DeleteImage(imageFile);
                    try
                    {
                        string fullPath = Request.MapPath(imageFile.ImagePath);
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
                _productService.DeleteProduct(product);
            }
            _categoryService.DeleteCategory(category);
            return Json("success", JsonRequestBehavior.AllowGet);

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

        public int GetUnreadMessagesCount()
        {
            return _messageService.GetUnreadMessagesCount();
        }

        public int GetRequestCount()
        {
            return _requestService.GetTotalRequests();
        }



        #region private functions
        public JsonResult CheckUniqueCategoryName(string name)
        {
            var result = _categoryService.GetCategoryByName(name);
            return Json(result == null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CheckUniqueCategoryNameInEdit(string name)
        {
            string _name = TempData["categoryName"].ToString();
            TempData["categoryName"] = _name;
            if (string.Compare(_name.ToLower(), name.ToLower(), true) == 0)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            var result = _categoryService.GetCategoryByName(name);
            return Json(result == null, JsonRequestBehavior.AllowGet);
        }

        private List<string> SetProductImages(IEnumerable<ImageFile> imageFiles)
        {
            List<string> productImages = new List<string>();

            if (imageFiles.Count() > 1)
            {
                foreach (var image in imageFiles)
                    productImages.Add(image.ImagePath);

                productImages.RemoveAt(0);
            }

            return productImages;
        }
        private string GetLoggedInUserId()
        {
            return User.Identity.GetUserId();
        }

        #endregion
    }
}