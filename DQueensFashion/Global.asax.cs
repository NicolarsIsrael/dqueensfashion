using DQueensFashion.Core.Model;
using DQueensFashion.Data;
using DQueensFashion.Data.Contract;
using DQueensFashion.Data.Implementation;
using DQueensFashion.Service.Contract;
using DQueensFashion.Service.Implementation;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DQueensFashion
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);



            //Create a new simple injector container
            var container = new Container();


            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
            container.Register<System.Data.Entity.DbContext>(() => new ApplicationDbContext(), Lifestyle.Scoped);

            container.Register<IUserStore<ApplicationUser>, MyUserStore>(Lifestyle.Scoped);
            container.Register<UserManager<ApplicationUser>, ApplicationUserManager>(Lifestyle.Scoped);


            container.Register<IAuthenticationManagerFactory, AuthenticationManagerFactory>();
            container.Register<IAuthenticationManager>(
                    () => HttpContext.Current.GetOwinContext().Authentication);

            //configure the services
            container.Register<IProductService, ProductService>(Lifestyle.Scoped);
            container.Register<ICategoryService, CategoryService>(Lifestyle.Scoped);
            

            //configure the Repos
            container.Register<IUnitOfWork, UnitOfWork>(Lifestyle.Scoped);
            container.Register<IProductRepo, ProductRepo>(Lifestyle.Scoped);
            container.Register<ICategoryRepo, CategoryRepo>(Lifestyle.Scoped);

            //registering Logic
            //container.Register<IMailer, Mailer>(Lifestyle.Scoped);

            //Register the MVC controllers to the container.
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
    }
}
