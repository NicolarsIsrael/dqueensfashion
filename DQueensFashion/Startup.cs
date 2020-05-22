using DQueensFashion.Core.Model;
using DQueensFashion.Data;
using DQueensFashion.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DQueensFashion.Startup))]
namespace DQueensFashion
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup i am creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists(AppConstant.AdminRole))
            {

                // first we create a admin role 
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = AppConstant.AdminRole;
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = AppConstant.AdminEmail;
                user.Email = AppConstant.AdminEmail;

                string userPWD = AppConstant.AdminPassword;

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, AppConstant.AdminRole);

                }
            }

            if (!roleManager.RoleExists(AppConstant.EmployeeRole))
            {
                // we create a employee role 
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = AppConstant.EmployeeRole;
                roleManager.Create(role);

            }

            if (!roleManager.RoleExists(AppConstant.CustomerRole))
            {
                // we create a customer role 
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = AppConstant.CustomerRole;
                roleManager.Create(role);
            }
        }
    }
}
