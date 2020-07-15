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

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var controllersUsingThisAttribute = ((AdminController)filterContext.Controller);
            filterContext.Controller.ViewBag.UnreadMessages = controllersUsingThisAttribute.GetUnreadMessagesCount();

            base.OnResultExecuting(filterContext);
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

    public class FaqSetGlobalVariable : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllersUsingThisAttribute = ((FaqController)filterContext.Controller);
            filterContext.Controller.ViewBag.CartNumber = controllersUsingThisAttribute.GetCartNumber();
            filterContext.Controller.ViewBag.Categories = controllersUsingThisAttribute.GetCategories();

            base.OnActionExecuting(filterContext);
        }
    }

    public class TermsOfUseSetGlobalVariable : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllersUsingThisAttribute = ((TermsOfUseController)filterContext.Controller);
            filterContext.Controller.ViewBag.CartNumber = controllersUsingThisAttribute.GetCartNumber();
            filterContext.Controller.ViewBag.Categories = controllersUsingThisAttribute.GetCategories();

            base.OnActionExecuting(filterContext);
        }
    }


    public class PrivacyPolicySetGlobalVariable : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllersUsingThisAttribute = ((PrivacyPolicyController)filterContext.Controller);
            filterContext.Controller.ViewBag.CartNumber = controllersUsingThisAttribute.GetCartNumber();
            filterContext.Controller.ViewBag.Categories = controllersUsingThisAttribute.GetCategories();

            base.OnActionExecuting(filterContext);
        }
    }

    public class NotFoundSetGlobalVariable : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllersUsingThisAttribute = ((NotFoundController)filterContext.Controller);
            filterContext.Controller.ViewBag.CartNumber = controllersUsingThisAttribute.GetCartNumber();
            filterContext.Controller.ViewBag.Categories = controllersUsingThisAttribute.GetCategories();

            base.OnActionExecuting(filterContext);
        }
    }

    public class ErrorSetGlobalVariable : ActionFilterAttribute, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllersUsingThisAttribute = ((ErrorController)filterContext.Controller);
            filterContext.Controller.ViewBag.CartNumber = controllersUsingThisAttribute.GetCartNumber();
            filterContext.Controller.ViewBag.Categories = controllersUsingThisAttribute.GetCategories();

            base.OnActionExecuting(filterContext);
        }
    }
}