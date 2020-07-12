using DQueensFashion.Controllers;
using DQueensFashion.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DQueensFashion.CustomFilters
{

    public class HomeSetGlobalVariable:ActionFilterAttribute,IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllersUsingThisAttribute = ((HomeController)filterContext.Controller);
            filterContext.Controller.ViewBag.CartNumber = controllersUsingThisAttribute.GetCartNumber();
            filterContext.Controller.ViewBag.Categories = controllersUsingThisAttribute.GetCategories();

            base.OnActionExecuting(filterContext);
        }

    }

    public class ProductSetGlobalVariable : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllersUsingThisAttribute = ((ProductController)filterContext.Controller);
            filterContext.Controller.ViewBag.CartNumber = controllersUsingThisAttribute.GetCartNumber();
            filterContext.Controller.ViewBag.Categories = controllersUsingThisAttribute.GetCategories();


            base.OnActionExecuting(filterContext);
        }

    }

    public class AccountSetGlobalVariable : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllersUsingThisAttribute = ((AccountController)filterContext.Controller);
            filterContext.Controller.ViewBag.CartNumber = controllersUsingThisAttribute.GetCartNumber();
            filterContext.Controller.ViewBag.Categories = controllersUsingThisAttribute.GetCategories();

            base.OnActionExecuting(filterContext);
        }

    }

    public class AdminSetGlobalVariable : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllersUsingThisAttribute = ((AdminController)filterContext.Controller);
            filterContext.Controller.ViewBag.CartNumber = controllersUsingThisAttribute.GetCartNumber();
            filterContext.Controller.ViewBag.Categories = controllersUsingThisAttribute.GetCategories();

            base.OnActionExecuting(filterContext);
        }

    }

    public class CustomerSetGlobalVariable : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllersUsingThisAttribute = ((CustomerController)filterContext.Controller);
            filterContext.Controller.ViewBag.CartNumber = controllersUsingThisAttribute.GetCartNumber();
            filterContext.Controller.ViewBag.Categories = controllersUsingThisAttribute.GetCategories();

            base.OnActionExecuting(filterContext);
        }

    }

    public class CartSetGlobalVariable : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllersUsingThisAttribute = ((CartController)filterContext.Controller);
            filterContext.Controller.ViewBag.CartNumber = controllersUsingThisAttribute.GetCartNumber();
            filterContext.Controller.ViewBag.Categories = controllersUsingThisAttribute.GetCategories();

            base.OnActionExecuting(filterContext);
        }

    }

    public class ManageSetGlobalVariable : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllersUsingThisAttribute = ((ManageController)filterContext.Controller);
            filterContext.Controller.ViewBag.CartNumber = controllersUsingThisAttribute.GetCartNumber();
            filterContext.Controller.ViewBag.Categories = controllersUsingThisAttribute.GetCategories();

            base.OnActionExecuting(filterContext);
        }

    }

    public class PaymentSetGlobalVariable : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllersUsingThisAttribute = ((PaymentController)filterContext.Controller);
            filterContext.Controller.ViewBag.CartNumber = controllersUsingThisAttribute.GetCartNumber();
            filterContext.Controller.ViewBag.Categories = controllersUsingThisAttribute.GetCategories();

            base.OnActionExecuting(filterContext);
        }
    }

    public class AboutUsSetGlobalVariable : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllersUsingThisAttribute = ((AboutUsController)filterContext.Controller);
            filterContext.Controller.ViewBag.CartNumber = controllersUsingThisAttribute.GetCartNumber();
            filterContext.Controller.ViewBag.Categories = controllersUsingThisAttribute.GetCategories();

            base.OnActionExecuting(filterContext);
        }
    }

    public class ContactUsSetGlobalVariable : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllersUsingThisAttribute = ((ContactUsController)filterContext.Controller);
            filterContext.Controller.ViewBag.CartNumber = controllersUsingThisAttribute.GetCartNumber();
            filterContext.Controller.ViewBag.Categories = controllersUsingThisAttribute.GetCategories();

            base.OnActionExecuting(filterContext);
        }
    }

}