using DQueensFashion.Core.Model;
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
    [Authorize(Roles =AppConstant.AdminRole)]
    public class AdminController : Controller
    {
        private readonly ICategoryService _categoryService;
        public AdminController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
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