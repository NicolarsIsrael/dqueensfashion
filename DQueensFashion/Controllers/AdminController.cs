using DQueensFashion.Core.Model;
using DQueensFashion.CustomFilters;
using DQueensFashion.Models;
using DQueensFashion.Service.Contract;
using DQueensFashion.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public AdminController(ICategoryService categoryService, IProductService productService, IOrderService orderService, IReviewService reviewService, IImageService imageService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _orderService = orderService;
            _reviewService = reviewService;
            _imageService = imageService;
        }
        // GET: Admin
        public ActionResult Index()
        {
            return RedirectToAction(nameof(Dashboard));
        }

        public ActionResult Dashboard()
        {
            return View();
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
        
        public ActionResult SearchCategories(string searchString)
        {
            IEnumerable<ViewCategoryViewModel> categories = _categoryService.GetAllCategories()
               .Select(c => new ViewCategoryViewModel()
               {
                   Id = c.Id,
                   Name = c.Name,
               }).OrderBy(c => c.Name);

            if (!string.IsNullOrEmpty(searchString))
                categories = categories.Where(c => c.Name.ToLower().Contains(searchString.ToLower())
                || string.Compare(c.Id.ToString(),searchString,true)==0
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
                    Name = p.Name,
                    Quantity = p.Quantity,
                    Price = p.Price.ToString(),
                    Discount = p.Discount,
                    SubTotal = p.SubTotal.ToString(),
                    Category = p.Category.Name,
                    MainImage = allImages.Where(image => image.ProductId == p.Id).Count() < 1 ?
                        AppConstant.DefaultProductImage :
                        allImages.Where(image => image.ProductId == p.Id).FirstOrDefault().ImagePath,
                    DateCreated = p.DateCreated,
                    DateCreatedString = p.DateCreated.ToString("dd/MMM/yyyy"),
                }).OrderBy(p=>p.Name).ToList();
            return View(products);
        }

        public ActionResult SearchProducts(string searchString)
        {
            var allImages = _imageService.GetAllImageFiles().ToList();

            IEnumerable<ViewProductsViewModel> products = _productService.GetAllProducts()
              .Select(p => new ViewProductsViewModel()
              {
                  Id = p.Id,
                  Name = p.Name,
                  Quantity = p.Quantity,
                  Price = p.Price.ToString(),
                  Discount = p.Discount,
                  SubTotal = p.SubTotal.ToString(),
                  Category = p.Category.Name,
                  MainImage = allImages.Where(image => image.ProductId == p.Id).Count() < 1 ?
                        AppConstant.DefaultProductImage :
                        allImages.Where(image => image.ProductId == p.Id).FirstOrDefault().ImagePath,
                  DateCreated = p.DateCreated,
                  DateCreatedString = p.DateCreated.ToString("dd/MMM/yyyy"),
              }).OrderBy(p => p.Name).ToList();

            if (!string.IsNullOrEmpty(searchString))
                products = products.Where(p => p.Name.ToLower().Contains(searchString.ToLower())
                || p.Category.ToLower().Contains(searchString.ToLower())
                || string.Compare(p.Quantity.ToString(), searchString, true) == 0
                || string.Compare(p.Price, searchString, true) == 0
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

            AddProductViewModel productModel = new AddProductViewModel()
            {
                Categories = categories,
                WaistLength = true,
                BurstSize = true,
                ShoulderLength = true,
                Length = false,
            };

            if (categoryId == AppConstant.CustomMadeCategoryId)
                productModel.CustomMadeCategory = true;

            return View(productModel);
        }

        [HttpPost]
        public ActionResult AddProduct(AddProductViewModel productModel)
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
                Price = Math.Round(productModel.Price, 2),
                Discount = Math.Round(productModel.Discount, 2),
                SubTotal = _productService.CalculateProductPrice(productModel.Price, productModel.Discount),
                Category = category,
                Tags = productModel.Tags != null ? String.Join(",", productModel.Tags) : "",
                ShoulderLength= productModel.ShoulderLength,
                BurstSize = productModel.BurstSize,
                WaistLength= productModel.WaistLength,
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
            return RedirectToAction(nameof(Products));
        }

        public ActionResult EditProduct(int id=0)
        {
            Product product = _productService.GetProductById(id);
            var allCategories = _categoryService.GetAllCategories().ToList();

            EditProductViewModel productModel = new EditProductViewModel()
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
                Tags=string.IsNullOrEmpty(product.Tags)?new List<string>():product.Tags.Split(',').ToList(),
                BurstSize=product.BurstSize.HasValue?product.BurstSize.Value:false,
                WaistLength=product.WaistLength.HasValue?product.WaistLength.Value:false,
                ShoulderLength=product.ShoulderLength.HasValue?product.ShoulderLength.Value:false,
                CustomMadeCategory = product.Category.Id == AppConstant.CustomMadeCategoryId ? true : false,
            };

            foreach (var category in productModel.Categories)
            {
                if (category.Id == product.CategoryId)
                    category.Selected = "selected";
            }

            return View(productModel);
        }

        [HttpPost]
        public ActionResult EditProduct(EditProductViewModel productModel)
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

                foreach(var c in productModel.Categories)
                {
                    if (c.Id == productModel.CategoryId)
                        c.Selected = "selected";
                }

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
            product.Price = Math.Round(productModel.Price,2);
            product.Discount = Math.Round(productModel.Discount, 2);
            product.SubTotal = Math.Round(_productService.CalculateProductPrice(productModel.Price, productModel.Discount));
            product.Category = category;
            product.Tags = productModel.Tags != null ? String.Join(",", productModel.Tags) : "";
            product.BurstSize = productModel.BurstSize;
            product.WaistLength = productModel.WaistLength;
            product.ShoulderLength = productModel.ShoulderLength;

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
                Price = product.Price.ToString(),
                Discount = product.Discount,
                SubTotal = product.SubTotal.ToString(),
                Quantity = product.Quantity,
                Category = product.Category.Name,
                Tags = product.Tags,
                DateCreatedString = product.DateCreated.ToString("dd/MMM/yyyy : hh-mm-ss"),
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
                        DateCreated = r.DateCreated.ToString("dd/MMM/yyyy"),
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
            return RedirectToAction(nameof(Products));
        }

        public PartialViewResult DeleteProductImage(int id)
        {
            ImageFile imageFile = _imageService.GetImageById(id);
            if (imageFile == null)
                throw new Exception();

            _imageService.DeleteImage(imageFile);

            IEnumerable<ImageViewModel> images = _imageService.GetImageFilesForProduct(imageFile.ProductId)
                .Select(image => new ImageViewModel()
                {
                    Id=image.Id,
                    ImagePath = image.ImagePath,
                });
            return PartialView("_productImages",images);
        }

        public ActionResult Orders()
        {
            IEnumerable<ViewOrderViewModel> orderModel = _orderService.GetAllOrders()
                .Select(order => new ViewOrderViewModel()
                {
                    OrderId = order.Id,
                    CustomerId = order.Customer.Id,
                    CustomerName = order.Customer.Fullname,
                    TotalAmount = order.TotalAmount,
                    TotalQuantity = order.TotalQuantity,
                    LineItems = order.LineItems
                        .Select(lineItem => new ViewLineItem()
                        {
                            Product = lineItem.Product.Name,
                            Quantity = lineItem.Quantity,
                            TotalAmount = lineItem.TotalAmount,
                        }),
                    OrderStatus= order.OrderStatus.ToString(),
                    DateCreated = order.DateCreated,
                    DateCreatedString = order.DateCreated.ToString("dd/MMM/yyyy : hh-mm-ss"),
                }).OrderBy(order=>order.DateCreated).ToList();

            return View(orderModel);
        }

        public ActionResult SearchOrders(string searchString)
        {
            
            IEnumerable<ViewOrderViewModel> orderModel = _orderService.GetAllOrders()
                .Select(order => new ViewOrderViewModel()
                {
                    OrderId = order.Id,
                    CustomerId = order.Customer.Id,
                    CustomerName = order.Customer.Fullname,
                    TotalAmount = order.TotalAmount,
                    TotalQuantity = order.TotalQuantity,
                    LineItems = order.LineItems
                        .Select(lineItem => new ViewLineItem()
                        {
                            Product = lineItem.Product.Name,
                            Quantity = lineItem.Quantity,
                            TotalAmount = lineItem.TotalAmount,
                        }),
                    OrderStatus = order.OrderStatus.ToString(),
                    DateCreated = order.DateCreated,
                    DateCreatedString = order.DateCreated.ToString("dd/MMM/yyyy : hh-mm-ss"),
                    LineItemConcatenatedString = string.Join(",",order.LineItems.Select(x=>x.Product.Name)),
                }).OrderBy(order => order.DateCreated).ToList();

            if (!string.IsNullOrEmpty(searchString))
                orderModel = orderModel.Where(order => order.CustomerName.ToLower().Contains(searchString.ToLower())
                || order.LineItemConcatenatedString.ToLower().Contains(searchString.ToLower())
                || order.OrderStatus.ToString().ToLower().Contains(searchString.ToLower())
                || (string.Compare(order.OrderId.ToString(),searchString,true)==0)
                ).ToList();

            return PartialView("_ordersTable",orderModel);
        }

        public ActionResult ProcessingOrders()
        {
            IEnumerable<ViewOrderViewModel> orderModel = _orderService.GetProcessingOrders()
                .Select(order => new ViewOrderViewModel()
                {
                    OrderId = order.Id,
                    CustomerId = order.Customer.Id,
                    CustomerName = order.Customer.Fullname,
                    TotalAmount = order.TotalAmount,
                    TotalQuantity = order.TotalQuantity,
                    LineItems = order.LineItems
                        .Select(lineItem => new ViewLineItem()
                        {
                            Product = lineItem.Product.Name,
                            Quantity = lineItem.Quantity,
                            TotalAmount = lineItem.TotalAmount,
                        }),
                    DateCreated = order.DateCreated,
                    DateCreatedString = order.DateCreated.ToString("dd/MMM/yyyy : hh-mm-ss"),
                    DateModified = order.DateModified,
                }).OrderBy(order => order.DateModified).ToList();

            return View(orderModel);
        }

        public ActionResult SearchProcessingOrders(string searchString)
        {
            try
            {
                IEnumerable<ViewOrderViewModel> orderModel = _orderService.GetProcessingOrders()
                       .Select(order => new ViewOrderViewModel()
                       {
                           OrderId = order.Id,
                           CustomerId = order.Customer.Id,
                           CustomerName = order.Customer.Fullname,
                           TotalAmount = order.TotalAmount,
                           TotalQuantity = order.TotalQuantity,
                           LineItems = order.LineItems
                               .Select(lineItem => new ViewLineItem()
                               {
                                   Product = lineItem.Product.Name,
                                   Quantity = lineItem.Quantity,
                                   TotalAmount = lineItem.TotalAmount,
                               }),
                           DateCreated = order.DateCreated,
                           DateCreatedString = order.DateCreated.ToString("dd/MMM/yyyy : hh-mm-ss"),
                           LineItemConcatenatedString = string.Join(",", order.LineItems.Select(x => x.Product.Name)),
                           DateModified = order.DateModified,
                       }).OrderBy(order => order.DateModified).ToList();

                if (!string.IsNullOrEmpty(searchString))
                    orderModel = orderModel.Where(order => order.CustomerName.ToLower().Contains(searchString.ToLower())
                    || order.LineItemConcatenatedString.ToLower().Contains(searchString.ToLower())
                    || (string.Compare(order.OrderId.ToString(), searchString, true) == 0)
                    ).ToList();

                
                return PartialView("_processingOrdersTable", orderModel);
            }
            catch (Exception ex)
            {
                var a = ex;
                throw;
            }
        }
        
        public ActionResult DeliveredOrders()
        {
            IEnumerable<ViewOrderViewModel> orderModel = _orderService.GetDeliveredOrders()
                .Select(order => new ViewOrderViewModel()
                {
                    OrderId = order.Id,
                    CustomerId = order.Customer.Id,
                    CustomerName = order.Customer.Fullname,
                    TotalAmount = order.TotalAmount,
                    TotalQuantity = order.TotalQuantity,
                    LineItems = order.LineItems
                        .Select(lineItem => new ViewLineItem()
                        {
                            Product = lineItem.Product.Name,
                            Quantity = lineItem.Quantity,
                            TotalAmount = lineItem.TotalAmount,
                        }),
                    DateCreated = order.DateCreated,
                    DateCreatedString = order.DateCreated.ToString("dd/MMM/yyyy : hh-mm-ss"),
                    DateModified = order.DateModified,
                }).OrderBy(order => order.DateModified).ToList();

            return View(orderModel);
        }

        public ActionResult SearchDeliveredOrders(string searchString)
        {

            IEnumerable<ViewOrderViewModel> orderModel = _orderService.GetDeliveredOrders()
                 .Select(order => new ViewOrderViewModel()
                 {
                     OrderId = order.Id,
                     CustomerId = order.Customer.Id,
                     CustomerName = order.Customer.Fullname,
                     TotalAmount = order.TotalAmount,
                     TotalQuantity = order.TotalQuantity,
                     LineItems = order.LineItems
                         .Select(lineItem => new ViewLineItem()
                         {
                             Product = lineItem.Product.Name,
                             Quantity = lineItem.Quantity,
                             TotalAmount = lineItem.TotalAmount,
                         }),
                     DateCreated = order.DateCreated,
                     DateCreatedString = order.DateCreated.ToString("dd/MMM/yyyy : hh-mm-ss"),
                     LineItemConcatenatedString = string.Join(",", order.LineItems.Select(x => x.Product.Name)),
                     DateModified = order.DateModified,
                 }).OrderBy(order => order.DateModified).ToList();



            if (!string.IsNullOrEmpty(searchString))
                orderModel = orderModel.Where(order => order.CustomerName.ToLower().Contains(searchString.ToLower())
                || order.LineItemConcatenatedString.ToLower().Contains(searchString.ToLower())
                || (string.Compare(order.OrderId.ToString(), searchString, true) == 0)
                ).ToList();

            return PartialView("_deliveredOrdersTable", orderModel);
        }

        public ActionResult ReturnedOrders()
        {
            IEnumerable<ViewOrderViewModel> orderModel = _orderService.GetReturnedOrders()
                .Select(order => new ViewOrderViewModel()
                {
                    OrderId = order.Id,
                    CustomerId = order.Customer.Id,
                    CustomerName = order.Customer.Fullname,
                    TotalAmount = order.TotalAmount,
                    TotalQuantity = order.TotalQuantity,
                    LineItems = order.LineItems
                        .Select(lineItem => new ViewLineItem()
                        {
                            Product = lineItem.Product.Name,
                            Quantity = lineItem.Quantity,
                            TotalAmount = lineItem.TotalAmount,
                        }),
                    DateCreated = order.DateCreated,
                    DateCreatedString = order.DateCreated.ToString("dd/MMM/yyyy : hh-mm-ss"),
                    DateModified = order.DateModified,
                }).OrderBy(order => order.DateModified).ToList();

            return View(orderModel);
        }

        public ActionResult SearchReturnedOrders(string searchString)
        {
            IEnumerable<ViewOrderViewModel> orderModel = _orderService.GetReturnedOrders()
                .Select(order => new ViewOrderViewModel()
                {
                    OrderId = order.Id,
                    CustomerId = order.Customer.Id,
                    CustomerName = order.Customer.Fullname,
                    TotalAmount = order.TotalAmount,
                    TotalQuantity = order.TotalQuantity,
                    LineItems = order.LineItems
                        .Select(lineItem => new ViewLineItem()
                        {
                            Product = lineItem.Product.Name,
                            Quantity = lineItem.Quantity,
                            TotalAmount = lineItem.TotalAmount,
                        }),
                    DateCreated = order.DateCreated,
                    DateCreatedString = order.DateCreated.ToString("dd/MMM/yyyy : hh-mm-ss"),
                    LineItemConcatenatedString = string.Join(",", order.LineItems.Select(x => x.Product.Name)),
                    DateModified = order.DateModified,
                }).OrderBy(order => order.DateModified).ToList();



            if (!string.IsNullOrEmpty(searchString))
                orderModel = orderModel.Where(order => order.CustomerName.ToLower().Contains(searchString.ToLower())
                || order.LineItemConcatenatedString.ToLower().Contains(searchString.ToLower())
                || (string.Compare(order.OrderId.ToString(), searchString, true) == 0)
                ).ToList();

            return PartialView("_returnedOrdersTable", orderModel);
        }

        public ActionResult DeletedOrders()
        {
            IEnumerable<ViewOrderViewModel> orderModel = _orderService.GetDeletedOrders()
               .Select(order => new ViewOrderViewModel()
               {
                   OrderId = order.Id,
                   CustomerId = order.Customer.Id,
                   CustomerName = order.Customer.Fullname,
                   TotalAmount = order.TotalAmount,
                   TotalQuantity = order.TotalQuantity,
                   LineItems = order.LineItems
                       .Select(lineItem => new ViewLineItem()
                       {
                           Product = lineItem.Product.Name,
                           Quantity = lineItem.Quantity,
                           TotalAmount = lineItem.TotalAmount,
                       }),
                   DateCreated = order.DateCreated,
                   DateCreatedString = order.DateCreated.ToString("dd/MMM/yyyy : hh-mm-ss"),
                   DateModified = order.DateModified,
               }).OrderBy(order => order.DateModified).ToList();

            return View(orderModel);
        }

        public ActionResult SearchDeletedOrders(string searchString)
        {
            IEnumerable<ViewOrderViewModel> orderModel = _orderService.GetDeletedOrders()
              .Select(order => new ViewOrderViewModel()
              {
                  OrderId = order.Id,
                  CustomerId = order.Customer.Id,
                  CustomerName = order.Customer.Fullname,
                  TotalAmount = order.TotalAmount,
                  TotalQuantity = order.TotalQuantity,
                  LineItems = order.LineItems
                      .Select(lineItem => new ViewLineItem()
                      {
                          Product = lineItem.Product.Name,
                          Quantity = lineItem.Quantity,
                          TotalAmount = lineItem.TotalAmount,
                      }),
                  DateCreated = order.DateCreated,
                  DateCreatedString = order.DateCreated.ToString("dd/MMM/yyyy : hh-mm-ss"),
                  LineItemConcatenatedString = string.Join(",", order.LineItems.Select(x => x.Product.Name)),
                  DateModified = order.DateModified,
              }).OrderBy(order => order.DateModified).ToList();



            if (!string.IsNullOrEmpty(searchString))
                orderModel = orderModel.Where(order => order.CustomerName.ToLower().Contains(searchString.ToLower())
                || order.LineItemConcatenatedString.ToLower().Contains(searchString.ToLower())
                || (string.Compare(order.OrderId.ToString(), searchString, true) == 0)
                ).ToList();

            return PartialView("_deletedOrdersTable", orderModel);
        }

        public ActionResult CompletedOrders()
        {
            IEnumerable<ViewOrderViewModel> orderModel = _orderService.GetCompletedOrders()
              .Select(order => new ViewOrderViewModel()
              {
                  OrderId = order.Id,
                  CustomerId = order.Customer.Id,
                  CustomerName = order.Customer.Fullname,
                  TotalAmount = order.TotalAmount,
                  TotalQuantity = order.TotalQuantity,
                  LineItems = order.LineItems
                      .Select(lineItem => new ViewLineItem()
                      {
                          Product = lineItem.Product.Name,
                          Quantity = lineItem.Quantity,
                          TotalAmount = lineItem.TotalAmount,
                      }),
                  DateCreated = order.DateCreated,
                  DateCreatedString = order.DateCreated.ToString("dd/MMM/yyyy : hh-mm-ss"),
                  DateModified = order.DateModified,
              }).OrderBy(order => order.DateModified).ToList();

            return View(orderModel);
        }

        public ActionResult SearchCompletedOrders(string searchString)
        {
            IEnumerable<ViewOrderViewModel> orderModel = _orderService.GetCompletedOrders()
               .Select(order => new ViewOrderViewModel()
               {
                   OrderId = order.Id,
                   CustomerId = order.Customer.Id,
                   CustomerName = order.Customer.Fullname,
                   TotalAmount = order.TotalAmount,
                   TotalQuantity = order.TotalQuantity,
                   LineItems = order.LineItems
                       .Select(lineItem => new ViewLineItem()
                       {
                           Product = lineItem.Product.Name,
                           Quantity = lineItem.Quantity,
                           TotalAmount = lineItem.TotalAmount,
                       }),
                   DateCreated = order.DateCreated,
                   DateCreatedString = order.DateCreated.ToString("dd/MMM/yyyy : hh-mm-ss"),
                   LineItemConcatenatedString = string.Join(",", order.LineItems.Select(x => x.Product.Name)),
                   DateModified = order.DateModified,
               }).OrderBy(order => order.DateModified).ToList();



            if (!string.IsNullOrEmpty(searchString))
                orderModel = orderModel.Where(order => order.CustomerName.ToLower().Contains(searchString.ToLower())
                || order.LineItemConcatenatedString.ToLower().Contains(searchString.ToLower())
                || (string.Compare(order.OrderId.ToString(), searchString, true) == 0)
                ).ToList();

            return PartialView("_completedOrdersTable", orderModel);
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

        #endregion
    }
}