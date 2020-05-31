using DQueensFashion.Core.Model;
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
    public class AdminController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;

        public AdminController(ICategoryService categoryService, IProductService productService, IOrderService orderService)
        {
            _categoryService = categoryService;
            _productService = productService;
            _orderService = orderService;
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

        public ActionResult ViewCategories()
        {
            IEnumerable<ViewCategoryViewModel> categories = _categoryService.GetAllCategories()
                .Select(c => new ViewCategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                }).OrderBy(c => c.Name);
            return View(categories);
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

                return RedirectToAction(nameof(ViewCategories));
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

                return RedirectToAction(nameof(ViewCategories));
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult ViewProducts()
        {
            IEnumerable<ViewProductsViewModel> products = _productService.GetAllProducts()
                .Select(p => new ViewProductsViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Quantity = p.Quantity.ToString(),
                    Price = p.Price.ToString(),
                    Category = p.Category.Name,
                    Image1 = p.ImagePath1,
                    DateCreated = p.DateCreated,
                    DateCreatedString = p.DateCreated.ToString("dd/MMM/yyyy : hh-mm-ss"),
                }).OrderBy(p=>p.DateCreated).ToList();
            return View(products);
        }

        public ActionResult AddProduct()
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
            };

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

            string imgPath1=string.Empty, imgPath2=string.Empty, imgPath3=string.Empty, imgPath4=string.Empty;
            //file 1
            if (productModel.ImageFile1 != null)
            {
                string fileName1 = FileService.GetFileName(productModel.ImageFile1);
                imgPath1 = "~/Content/Images/Products/" + fileName1;
                fileName1 = Path.Combine(Server.MapPath("~/Content/Images/Products/"), fileName1);
                FileService.SaveImage(productModel.ImageFile1, fileName1);
            }
            //file 2
            if (productModel.ImageFile2 != null)
            {
                string fileName2 = FileService.GetFileName(productModel.ImageFile2);
                imgPath2 = "~/Content/Images/Products/" + fileName2;
                fileName2 = Path.Combine(Server.MapPath("~/Content/Images/Products/"), fileName2);
                FileService.SaveImage(productModel.ImageFile2, fileName2);
            }
            //file 3
            if (productModel.ImageFile3 != null)
            {
                string fileName3 = FileService.GetFileName(productModel.ImageFile3);
                imgPath3 = "~/Content/Images/Products/" + fileName3;
                fileName3 = Path.Combine(Server.MapPath("~/Content/Images/Products/"), fileName3);
                FileService.SaveImage(productModel.ImageFile3, fileName3);
            }
            //file 4
            if (productModel.ImageFile4 != null)
            {
                string fileName4 = FileService.GetFileName(productModel.ImageFile4);
                imgPath4 = "~/Content/Images/Products/" + fileName4;
                fileName4 = Path.Combine(Server.MapPath("~/Content/Images/Products/"), fileName4);
                FileService.SaveImage(productModel.ImageFile4, fileName4);
            }

            Product product = new Product()
            {
                Name = productModel.Name,
                Description = productModel.Description,
                Quantity = productModel.Quantity,
                Price = productModel.Price,
                ImagePath1 = string.IsNullOrEmpty(imgPath1) ? AppConstant.DefaultProductImage : imgPath1,
                ImagePath2 = imgPath2,
                ImagePath3 = imgPath3,
                ImagePath4 = imgPath4,
                Category = category,
                Tags = productModel.Tags != null ? String.Join(",", productModel.Tags) : "",
            };

            _productService.AddProduct(product);
            return RedirectToAction(nameof(ViewProducts));
        }

        public ActionResult EditProduct(int id=0)
        {
            Product product = _productService.GetProductById(id);
            var allCategories = _categoryService.GetAllCategories().ToList();
            allCategories.Remove(product.Category);
            EditProductViewModel productModel = new EditProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Quantity = product.Quantity,
                Price = product.Price,
                PreviousCategory = new CategoryNameAndId()
                {
                    Id = product.Category.Id,
                    Name = product.Category.Name,
                },
                Categories = allCategories.Select(c => new CategoryNameAndId()
                {
                    Id = c.Id,
                    Name = c.Name,
                }),
                ImagePath1 = string.IsNullOrEmpty(product.ImagePath1) ? AppConstant.DefaultProductImage : product.ImagePath1,
                ImagePath2 = string.IsNullOrEmpty(product.ImagePath2) ? AppConstant.DefaultProductImage : product.ImagePath2,
                ImagePath3 = string.IsNullOrEmpty(product.ImagePath3) ? AppConstant.DefaultProductImage : product.ImagePath3,
                ImagePath4 = string.IsNullOrEmpty(product.ImagePath4) ? AppConstant.DefaultProductImage : product.ImagePath4,
                Tags=string.IsNullOrEmpty(product.Tags)?new List<string>():product.Tags.Split(',').ToList(),
            };
            
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

                var allCategories = _categoryService.GetAllCategories().ToList();
                allCategories.Remove(_category);

                productModel.PreviousCategory = new CategoryNameAndId()
                {
                    Id = _category.Id,
                    Name = _category.Name,
                };
                productModel.Categories = allCategories.Select(c => new CategoryNameAndId()
                {
                    Id = c.Id,
                    Name = c.Name,
                }).ToList();
                productModel.ImagePath1 = string.IsNullOrEmpty(_product.ImagePath1) ? AppConstant.DefaultProductImage : _product.ImagePath1;
                productModel.ImagePath2 = string.IsNullOrEmpty(_product.ImagePath2) ? AppConstant.DefaultProductImage : _product.ImagePath2;
                productModel.ImagePath3 = string.IsNullOrEmpty(_product.ImagePath3) ? AppConstant.DefaultProductImage : _product.ImagePath3;
                productModel.ImagePath4 = string.IsNullOrEmpty(_product.ImagePath4) ? AppConstant.DefaultProductImage : _product.ImagePath4;
                productModel.Tags = productModel.Tags == null ? new List<string>() : productModel.Tags;

                return View(productModel);
            }

            Product product = _productService.GetProductById(productModel.Id);
            if (product == null)
                throw new Exception();

            Category category = _categoryService.GetCategoryById(productModel.CategoryId);
            if (category == null)
                throw new Exception();

            string imgPath1 = string.Empty, imgPath2 = string.Empty, imgPath3 = string.Empty, imgPath4 = string.Empty;
            //file 1
            if (productModel.ImageFile1 != null)
            {
                string fileName1 = FileService.GetFileName(productModel.ImageFile1);
                imgPath1 = "~/Content/Images/Products/" + fileName1;
                fileName1 = Path.Combine(Server.MapPath("~/Content/Images/Products/"), fileName1);
                FileService.SaveImage(productModel.ImageFile1, fileName1);
            }
            //file 2
            if (productModel.ImageFile2 != null)
            {
                string fileName2 = FileService.GetFileName(productModel.ImageFile2);
                imgPath2 = "~/Content/Images/Products/" + fileName2;
                fileName2 = Path.Combine(Server.MapPath("~/Content/Images/Products/"), fileName2);
                FileService.SaveImage(productModel.ImageFile2, fileName2);
            }
            //file 3
            if (productModel.ImageFile3 != null)
            {
                string fileName3 = FileService.GetFileName(productModel.ImageFile3);
                imgPath3 = "~/Content/Images/Products/" + fileName3;
                fileName3 = Path.Combine(Server.MapPath("~/Content/Images/Products/"), fileName3);
                FileService.SaveImage(productModel.ImageFile3, fileName3);
            }
            //file 4
            if (productModel.ImageFile4 != null)
            {
                string fileName4 = FileService.GetFileName(productModel.ImageFile4);
                imgPath4 = "~/Content/Images/Products/" + fileName4;
                fileName4 = Path.Combine(Server.MapPath("~/Content/Images/Products/"), fileName4);
                FileService.SaveImage(productModel.ImageFile4, fileName4);
            }

            product.Name = productModel.Name;
            product.Description = productModel.Description;
            product.Quantity = productModel.Quantity;
            product.Price = productModel.Price;
            product.Category = category;
            product.Tags = productModel.Tags != null ? String.Join(",", productModel.Tags) : "";
            product.ImagePath1 = string.IsNullOrEmpty(imgPath1) ? product.ImagePath1 : imgPath1;
            product.ImagePath2 = string.IsNullOrEmpty(imgPath2) ? product.ImagePath2 : imgPath2;
            product.ImagePath3 = string.IsNullOrEmpty(imgPath3) ? product.ImagePath3 : imgPath3;
            product.ImagePath4 = string.IsNullOrEmpty(imgPath4) ? product.ImagePath4 : imgPath4;

            _productService.UpdateProduct(product);
            return RedirectToAction(nameof(ViewProducts));
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
                Image1 = string.IsNullOrEmpty(product.ImagePath1)?"":product.ImagePath1,
                Image2 = string.IsNullOrEmpty(product.ImagePath2) ? "" : product.ImagePath2,
                Image3 = string.IsNullOrEmpty(product.ImagePath3) ? "" : product.ImagePath3,
                Image4 = string.IsNullOrEmpty(product.ImagePath4) ? "" : product.ImagePath4,
            };

            return View(productModel);
        }

        public ActionResult ViewOrders()
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

        public ActionResult ViewProcessingOrders()
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

        public ActionResult ViewDeliveredOrders()
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

        public ActionResult ViewReturnedOrders()
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

        public ActionResult ViewDeletedOrders()
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
        public ActionResult ViewCompletedOrders()
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

        public ActionResult DeliverOrder(int id)
        {
            Order order = _orderService.GetOrderById(id);
            if (order == null)
                throw new Exception();

            order.OrderStatus = OrderStatus.Delivered;
            _orderService.UpdateOrder(order);

            return RedirectToAction(nameof(ViewDeliveredOrders));
        }

        public ActionResult ReturnOrder(int id)
        {
            Order order = _orderService.GetOrderById(id);
            if (order == null)
                throw new Exception();

            order.OrderStatus = OrderStatus.Returned;
            _orderService.UpdateOrder(order);

            return RedirectToAction(nameof(ViewReturnedOrders));
        }

        public ActionResult CompleteOrder(int id)
        {
            Order order = _orderService.GetOrderById(id);
            if (order == null)
                throw new Exception();

            order.OrderStatus = OrderStatus.Completed;
            _orderService.UpdateOrder(order);

            return RedirectToAction(nameof(ViewCompletedOrders));
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

            return RedirectToAction(nameof(ViewOrders));
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

        #endregion
    }
}