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




        #region private functions
        public JsonResult CheckUniqueCategoryName(string name)
        {
            var result = _categoryService.GetCategoryByName(name);
            return Json(result == null, JsonRequestBehavior.AllowGet);
        }



        //public JsonResult EnsureUniqueCourseCodeInEdit(string courseCode)
        //{
        //    string _courseCode = TempData["Course"].ToString();
        //    if (string.Compare(_courseCode.ToLower(), courseCode.ToLower(), true) == 0)
        //    {
        //        TempData["Course"] = _courseCode;
        //        return Json(true, JsonRequestBehavior.AllowGet);
        //    }

        //    TempData["Course"] = _courseCode;
        //    var result = _courseService.GetCourseByCourseCode(courseCode);
        //    return Json(result == null, JsonRequestBehavior.AllowGet);
        //}
        #endregion
    }
}