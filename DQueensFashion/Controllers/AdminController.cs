﻿using DQueensFashion.Core.Model;
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
        public AdminController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
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
                    Description = p.Description,
                    Quantity = p.Quantity.ToString(),
                    Price = p.Price.ToString(),
                    Category = p.Category.Name,
                    Image1 = p.ImagePath1,
                    Image2 = p.ImagePath2,
                    Image3 = p.ImagePath3,
                    Image4 = p.ImagePath4,
                    DateCreated = p.DateCreated,
                    DateCreatedString = p.DateCreated.ToString("dd-MMM-yyyy : hh-mm-ss"),
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
            };

            _productService.AddProduct(product);
            return RedirectToAction(nameof(ViewProducts));
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