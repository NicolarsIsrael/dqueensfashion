using DQueensFashion.Core.Model;
using DQueensFashion.Data;
using DQueensFashion.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;

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



                var categories = new List<Category>()
                 {
                    new Category{ Name="Ankara",IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now},
                    new Category{ Name="Lace",IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now},
                    new Category{ Name="Baby-Wears",IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now},
                    new Category{ Name="Custom-Made",IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now},
                };

                categories.ForEach(c => context.Category.Add(c));
                context.SaveChanges();

                var products = new List<Product>()
            {
                new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(1),
                    Name="2 yards ankara",Price = 20,Quantity=10,Tags="ankara,african,male",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2017/08/01/08/29/people-2563491_960_720.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2020/05/26/15/42/eagle-5223559_960_720.jpg",
                    ImagePath3="https://cdn.pixabay.com/photo/2014/05/03/00/56/summerfield-336672_960_720.jpg",
                    ImagePath4="https://cdn.pixabay.com/photo/2020/05/28/19/01/daisies-5232284_960_720.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(1),
                    Name="Blue flowers ankara",Price = 10,Quantity=15,Tags="ankara,african,male",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2016/10/16/13/44/young-woman-1745173__340.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2018/01/11/09/39/woman-3075704_960_720.jpg",
                    ImagePath3="https://cdn.pixabay.com/photo/2018/01/13/19/39/fashion-3080644_960_720.jpg",
                    ImagePath4="https://cdn.pixabay.com/photo/2018/02/24/20/40/fashion-3179178__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(1),
                    Name="Sisi dress",Price = 10,Quantity=10,Tags="female,dress",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2014/08/08/20/55/worried-girl-413690__340.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2016/06/29/04/17/wedding-dresses-1485984__340.jpg",
                    ImagePath3="https://cdn.pixabay.com/photo/2016/06/29/08/41/wedding-dresses-1486256__340.jpg",
                    ImagePath4="",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(1),
                    Name="Denim jacket",Price = 12,Quantity=17,Tags="Denim attire",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2017/08/06/20/11/people-2595862__340.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2016/01/07/04/25/girl-1125318__340.jpg",
                    ImagePath3="https://cdn.pixabay.com/photo/2016/11/14/04/57/young-1822656__340.jpg",
                    ImagePath4="",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(1),
                    Name="Debi kimono jacket",Price = 10,Quantity=12,Tags="jacket",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://media.istockphoto.com/photos/buckle-up-youre-about-to-get-married-picture-id1173486515?b=1&k=6&m=1173486515&s=170667a&w=0&h=LsjHYcxJ3dMtWMGes4ygxeROKZsXMxE-Dm_CW23yOdA=",
                    ImagePath2="",
                    ImagePath3="https://media.istockphoto.com/photos/buckle-up-youre-about-to-get-married-picture-id1173486515?b=1&k=6&m=1173486515&s=170667a&w=0&h=LsjHYcxJ3dMtWMGes4ygxeROKZsXMxE-Dm_CW23yOdA=",
                    ImagePath4="https://cdn.pixabay.com/photo/2016/06/29/08/42/wedding-dresses-1486260__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(1),
                    Name="Cheta hilo top",Price = 20,Quantity=10,Tags="female,dress",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2018/01/11/09/52/three-3075752__340.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2018/01/11/09/52/three-3075752__340.jpg",
                    ImagePath3="",
                    ImagePath4="https://cdn.pixabay.com/photo/2018/02/24/20/41/beautiful-3179182__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(1),
                    Name="Chizo hilo top",Price = 20,Quantity=10,Tags="female,dress,top",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2015/11/07/11/46/fashion-1031469__340.jpg",
                    ImagePath2="",
                    ImagePath3="",
                    ImagePath4="https://cdn.pixabay.com/photo/2016/11/12/20/00/under-water-1819586__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(1),
                    Name="Blazzer suit",Price = 40,Quantity=14,Tags="suit,coperate,male",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2015/01/12/10/44/portrait-597173__340.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2016/11/19/20/17/catwalk-1840941__340.jpg",
                    ImagePath3="https://cdn.pixabay.com/photo/2016/11/14/03/30/bride-1822488__340.jpg",
                    ImagePath4="https://cdn.pixabay.com/photo/2016/06/29/08/38/wedding-dresses-1486242__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(1),
                    Name="Kamdi dress",Price = 12,Quantity=5,Tags="female,dress",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2015/09/02/12/28/fashion-918446__340.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2020/02/05/11/06/portrait-4820889__340.jpg",
                    ImagePath3="",
                    ImagePath4="",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(1),
                    Name="dummy text of the printing",Price = 13,Quantity=5,Tags="shoes,african,male",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2017/03/01/05/43/pink-shoes-2107618__340.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2017/03/01/05/38/pink-shoes-2107616__340.jpg",
                    ImagePath3="",
                    ImagePath4="https://cdn.pixabay.com/photo/2015/11/07/11/06/shoes-1030823__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(1),
                    Name="five centuries",Price = 10,Quantity=20,Tags="kids,european,male",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2017/01/10/20/34/dress-1970144__340.jpg",
                    ImagePath2="",
                    ImagePath3="",
                    ImagePath4="https://cdn.pixabay.com/photo/2015/09/23/03/07/belt-952834__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(1),
                    Name="long trousers",Price = 17,Quantity=23,Tags="ankara,african,male",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2020/05/15/10/21/fashion-5173136__340.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2020/05/18/10/24/pants-5185567__340.jpg",
                    ImagePath3="",
                    ImagePath4="https://cdn.pixabay.com/photo/2015/03/26/09/52/dress-690496__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(1),
                    Name="Jean trousers",Price = 13,Quantity=14,Tags="ankara,african,male",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2015/03/26/09/52/dress-690496__340.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2017/06/15/11/31/leg-2405061__340.jpg",
                    ImagePath3="https://cdn.pixabay.com/photo/2017/06/15/11/31/leg-2405061__340.jpg",
                    ImagePath4="https://cdn.pixabay.com/photo/2017/08/07/13/13/black-and-white-2603717__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(1),
                    Name="desktop publishing",Price = 30,Quantity=10,Tags="ankara,african,male",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2019/07/18/05/38/tribal-background-4345552__340.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2020/01/30/06/39/digital-paper-4804500__340.jpg",
                    ImagePath3="https://cdn.pixabay.com/photo/2017/01/20/09/06/african-fabric-1994340__340.jpg",
                    ImagePath4="https://cdn.pixabay.com/photo/2017/10/24/04/19/texture-2883377__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(1),
                    Name="Baby shoes",Price = 15,Quantity=10,Tags="baby,shoes,male,female",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://images.unsplash.com/photo-1515886657613-9f3515b0c78f?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=500&q=60",
                    ImagePath2="https://images.unsplash.com/photo-1483985988355-763728e1935b?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=500&q=60",
                    ImagePath3="https://images.unsplash.com/photo-1512436991641-6745cdb1723f?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=500&q=60",
                    ImagePath4="",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(2),
                    Name="Fancy lace",Price = 26,Quantity=12,Tags="ankara,african,male",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://images.unsplash.com/photo-1509631179647-0177331693ae?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=500&q=60",
                    ImagePath2="https://images.unsplash.com/photo-1503342217505-b0a15ec3261c?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=500&q=60",
                    ImagePath3="https://images.unsplash.com/photo-1495121605193-b116b5b9c5fe?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=500&q=60",
                    ImagePath4="https://images.unsplash.com/photo-1495121605193-b116b5b9c5fe?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=500&q=60",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(1),
                    Name="2 layer cotton",Price = 13,Quantity=5,Tags="ankara,african,male",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://images.unsplash.com/photo-1505734169265-a86113baa6c5?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=500&q=60",
                    ImagePath2="https://images.unsplash.com/photo-1548549557-dbe9946621da?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=500&q=60",
                    ImagePath3="",
                    ImagePath4="",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(2),
                    Name="5 yard lace",Price = 20,Quantity=10,Tags="lace,female,african,male",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://unsplash.com/photos/QCPXRvl9Ylc",
                    ImagePath2="https://cdn.pixabay.com/photo/2017/10/24/04/19/texture-2883375__340.jpg",
                    ImagePath3="https://cdn.pixabay.com/photo/2018/04/30/06/15/doll-3361861__340.jpg",
                    ImagePath4="https://cdn.pixabay.com/photo/2018/04/30/06/15/doll-3361861__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(2),
                    Name="Beautiful yellow lace",Price = 10,Quantity=14,Tags="lace,african,male",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2016/11/14/04/57/young-1822656__340.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2015/03/26/09/54/people-690547__340.jpg",
                    ImagePath3="",
                    ImagePath4="https://cdn.pixabay.com/photo/2016/11/14/03/02/asia-1822454__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(2),
                    Name="Green authentic lace",Price = 40,Quantity=7,Tags="ankara,african,male",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2016/05/17/22/16/baby-1399332__340.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2016/07/11/15/43/pretty-woman-1509956__340.jpg",
                    ImagePath3="https://cdn.pixabay.com/photo/2017/05/13/12/40/fashion-2309519__340.jpg",
                    ImagePath4="https://cdn.pixabay.com/photo/2016/01/19/17/48/woman-1149911__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(2),
                    Name="Original lace wear",Price = 20,Quantity=11,Tags="ankara,african,male",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2017/08/30/17/27/business-woman-2697954__340.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2016/11/29/09/38/adult-1868750__340.jpg",
                    ImagePath3="",
                    ImagePath4="https://cdn.pixabay.com/photo/2016/11/18/17/08/fashion-1835871__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(2),
                    Name="Blue ordinary lace",Price = 9,Quantity=13,Tags="ankara,african,male",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2017/12/15/18/50/isolated-3021541__340.png",
                    ImagePath2="",
                    ImagePath3="https://cdn.pixabay.com/photo/2017/11/02/14/26/model-2911330__340.jpg",
                    ImagePath4="",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(2),
                    Name="Fancy brown lace",Price = 23,Quantity=5,Tags="ankara,african,male",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2015/09/02/12/57/woman-918784__340.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2016/11/19/17/45/blur-1840538__340.jpg",
                    ImagePath3="",
                    ImagePath4="",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(2),
                    Name="2 yards lacy suit",Price = 30,Quantity=10,Tags="lace,african,female",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2013/07/12/15/40/gown-150290__340.png",
                    ImagePath2="https://cdn.pixabay.com/photo/2018/03/03/03/32/female-3194839__340.png",
                    ImagePath3="https://cdn.pixabay.com/photo/2016/06/08/04/16/woman-1443139__340.png",
                    ImagePath4="https://cdn.pixabay.com/photo/2016/06/11/16/51/prom-1450373__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(2),
                    Name="Cotton lace",Price = 23,Quantity=4,Tags="ankara,african,male",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2016/06/11/16/51/prom-1450373__340.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2013/07/13/12/49/celebration-160402__340.png",
                    ImagePath3="https://cdn.pixabay.com/photo/2013/07/13/12/49/celebration-160402__340.png",
                    ImagePath4="https://cdn.pixabay.com/photo/2017/07/31/19/21/people-2560216__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(3),
                    Name="baby cap",Price = 10,Quantity=15,Tags="baby,cap,male",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://media.istockphoto.com/photos/mother-caring-after-her-babys-skin-picture-id1178838829?b=1&k=6&m=1178838829&s=170667a&w=0&h=wKtS7Yoz5TmHmG60-WGkZvdGUv3peFwVoI--HP7m8c0=",
                    ImagePath2="https://cdn.pixabay.com/photo/2016/01/31/05/49/happy-1170988__340.png",
                    ImagePath3="https://cdn.pixabay.com/photo/2016/10/24/04/23/baby-1765356__340.jpg",
                    ImagePath4="https://cdn.pixabay.com/photo/2017/11/06/13/44/cap-2923677__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(3),
                    Name="Baby shoes",Price = 10,Quantity=15,Tags="baby,female,male",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://media.istockphoto.com/photos/beautiful-pregnant-mother-on-baby-room-picture-id1175914976?b=1&k=6&m=1175914976&s=170667a&w=0&h=PUT2KFmc6p2DbgisQuSp43hU4j3Hy2eb-Wk1CqJ8EPI=",
                    ImagePath2="https://cdn.pixabay.com/photo/2020/01/24/08/52/bear-4789688__340.jpg",
                    ImagePath3="https://cdn.pixabay.com/photo/2016/09/27/12/31/baby-1698234__340.jpg",
                    ImagePath4="https://cdn.pixabay.com/photo/2016/09/27/12/31/shoes-1698239__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(3),
                    Name="Baby overall",Price = 14,Quantity=7,Tags="baby,female,male",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2018/06/15/03/31/diapers-3476133__340.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2019/11/13/21/59/infant-4624862__340.jpg",
                    ImagePath3="",
                    ImagePath4="",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(3),
                    Name="Baby feeder",Price = 6,Quantity=10,Tags="baby",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2020/01/24/08/52/cute-4789689__340.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2017/03/01/23/04/tapes-2109890__340.jpg",
                    ImagePath3="https://cdn.pixabay.com/photo/2017/03/01/23/10/tapes-2109903__340.jpg",
                    ImagePath4="https://cdn.pixabay.com/photo/2017/03/01/23/07/tape-2109900__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(3),
                    Name="Green baby shoes",Price = 8,Quantity=16,Tags="babywears,male",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2016/11/09/20/51/christmas-1812692__340.jpg",
                    ImagePath2="",
                    ImagePath3="",
                    ImagePath4="https://cdn.pixabay.com/photo/2017/11/07/17/35/baby-2927579__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(3),
                    Name="Red baby shorts",Price = 10,Quantity=6,Tags="baby,babywears,male,female",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2017/02/08/02/56/booties-2047596__340.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2017/11/11/18/43/fashion-2939989__340.jpg",
                    ImagePath3="https://cdn.pixabay.com/photo/2016/11/09/20/51/christmas-1812692__340.jpg",
                    ImagePath4="",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(3),
                    Name="Baby red top",Price = 7,Quantity=13,Tags="baby,babywears,male,female",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2018/04/19/19/49/child-3334093__340.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2012/05/04/09/26/bunny-46886__340.png",
                    ImagePath3="https://cdn.pixabay.com/photo/2016/04/14/08/40/newborn-1328454__340.jpg",
                    ImagePath4="https://cdn.pixabay.com/photo/2014/09/23/06/02/brothers-457234__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(3),
                    Name="Baby blue cap",Price = 34,Quantity=14,Tags="baby,babywears,male,female",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2012/02/16/12/08/girl-13422__340.jpg",
                    ImagePath2="",
                    ImagePath3="",
                    ImagePath4="",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(3),
                    Name="Baby layerd shoes",Price = 13,Quantity=14,Tags="ankara,african,male",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2018/02/18/00/13/child-3161273__340.jpg",
                    ImagePath2="",
                    ImagePath3="",
                    ImagePath4="",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(4),
                    Name="Custom made red dress",Price = 12,Quantity=6,Tags="custommade",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2017/12/13/09/23/model-3016382__340.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2017/12/13/09/23/model-3016384__340.jpg",
                    ImagePath3="",
                    ImagePath4="",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(4),
                    Name="Custom made shoe",Price = 20,Quantity=13,Tags="custommade",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2017/04/09/18/54/shoes-2216498__340.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2015/02/14/02/18/marriage-636018__340.jpg",
                    ImagePath3="https://cdn.pixabay.com/photo/2016/10/18/08/13/travel-1749508__340.jpg",
                    ImagePath4="https://cdn.pixabay.com/photo/2016/06/03/17/35/shoe-1433925__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(4),
                    Name="Custom made blue attire",Price = 23,Quantity=14,Tags="custommade",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2020/02/29/18/09/girl-4890772__340.png",
                    ImagePath2="https://cdn.pixabay.com/photo/2017/01/31/23/14/clothes-2028064__340.png",
                    ImagePath3="https://cdn.pixabay.com/photo/2015/06/08/15/25/bride-802060__340.jpg",
                    ImagePath4="https://cdn.pixabay.com/photo/2017/08/01/08/28/people-2563486__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(1),
                    Name="Custom made berret",Price = 20,Quantity=10,Tags="custommade",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://media.istockphoto.com/photos/attractive-young-woman-in-red-leather-berret-and-silk-dress-picture-id519213031?b=1&k=6&m=519213031&s=170667a&w=0&h=XcCnK9nfM1n9FU5GHyP8VFO4CE2T0-ReGWSvHa5xILc=",
                    ImagePath2="",
                    ImagePath3="https://cdn.pixabay.com/photo/2015/04/13/13/48/hat-720599__340.jpg",
                    ImagePath4="",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(4),
                    Name="Custom made blue silk berret",Price = 20,Quantity=10,Tags="custommade",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://media.istockphoto.com/photos/senior-couple-in-venice-picture-id459009393?b=1&k=6&m=459009393&s=170667a&w=0&h=hNXbLm7zBqiPiPZMmegG63oeZYiFK_A2J0tHBOdNUVo=",
                    ImagePath2="https://media.istockphoto.com/photos/man-in-costume-sitting-at-a-bistro-table-picture-id533438032?b=1&k=6&m=533438032&s=170667a&w=0&h=y443Y_En9vq3JNTXsyaNmKWfimmdQ0c4XUcS4JauYOo=",
                    ImagePath3="",
                    ImagePath4="",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(4),
                    Name="Custom blue joggers",Price = 14,Quantity=7,Tags="custommade",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2017/08/31/08/48/young-woman-2699780__340.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2017/08/31/08/48/young-woman-2699780__340.jpg",
                    ImagePath3="https://cdn.pixabay.com/photo/2017/10/21/16/46/running-2875180__340.jpg",
                    ImagePath4="",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(4),
                    Name="Custom made red skirt",Price = 13,Quantity=8,Tags="custommade",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://media.istockphoto.com/photos/woman-dancing-in-the-city-street-on-a-sunny-summer-day-picture-id1142311415?b=1&k=6&m=1142311415&s=170667a&w=0&h=-FQj4ZnHfFVD7TJN-u8-V2eD7bFjYdxmRS-1L1X5Hq0=",
                    ImagePath2="https://cdn.pixabay.com/photo/2016/10/20/08/36/woman-1754895__340.jpg",
                    ImagePath3="https://cdn.pixabay.com/photo/2016/12/23/22/19/photoshoot-1928086__340.jpg",
                    ImagePath4="https://cdn.pixabay.com/photo/2017/08/06/08/01/people-2590092__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(4),
                    Name="Custom shoes",Price = 16,Quantity=8,Tags="custommade",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2016/01/19/18/06/brown-shoes-1150071__340.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2016/11/19/18/06/feet-1840619__340.jpg",
                    ImagePath3="https://cdn.pixabay.com/photo/2014/12/31/11/41/shoes-584850__340.jpg",
                    ImagePath4="https://cdn.pixabay.com/photo/2016/01/19/17/47/hiking-1149891__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(1),
                    Name="10 yards ankara",Price = 40,Quantity=15,Tags="ankara,african,male",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://images.unsplash.com/photo-1566659059394-519e944173ee?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=500&q=60",
                    ImagePath2="https://images.unsplash.com/photo-1566659059394-519e944173ee?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=500&q=60",
                    ImagePath3="https://media.istockphoto.com/photos/jamaa-el-fna-market-picture-id1147478209?b=1&k=6&m=1147478209&s=170667a&w=0&h=Iisu7hoGbxSr6ldHbdAnzETfeY-4Jp0iNENmk8WxM3w=",
                    ImagePath4="https://cdn.pixabay.com/photo/2017/01/20/09/06/african-fabric-1994340__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(1),
                    Name="5 yards green ankara",Price = 24,Quantity=13,Tags="ankara,african,male",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2020/01/30/06/32/digital-paper-4804481__340.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2017/11/06/06/25/lady-2922848__340.jpg",
                    ImagePath3="",
                    ImagePath4="https://cdn.pixabay.com/photo/2017/10/24/04/19/texture-2883375__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(1),
                    Name="7 yards orange ankara",Price = 14,Quantity=9,Tags="ankara,african,male",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://images.unsplash.com/photo-1580974582391-a6649c82a85f?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=500&q=60",
                    ImagePath2="https://media.istockphoto.com/photos/provence-france-african-wax-textiles-for-sale-at-market-picture-id1158291245?b=1&k=6&m=1158291245&s=170667a&w=0&h=ivOb4OzUohxlRLS_EsyQjifGHdH12leP5HMz9pYef-A=",
                    ImagePath3="https://cdn.pixabay.com/photo/2017/10/24/04/20/texture-2883381__340.jpg",
                    ImagePath4="https://cdn.pixabay.com/photo/2017/10/24/04/19/texture-2883379__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(2),
                    Name="Affordable jump suits",Price = 23,Quantity=10,Tags="Lace",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2017/06/08/02/14/girl-2382231__340.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2017/06/08/02/14/girl-2382231__340.jpg",
                    ImagePath3="https://cdn.pixabay.com/photo/2020/05/16/17/17/sports-5178367__340.jpg",
                    ImagePath4="https://cdn.pixabay.com/photo/2017/10/18/11/47/hat-2863852__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(1),
                    Name="Baby velvet shoes",Price = 17,Quantity=8,Tags="Baby",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2017/10/18/11/47/hat-2863852__340.jpg ",
                    ImagePath2="https://cdn.pixabay.com/photo/2014/10/22/17/25/stretching-498256__340.jpg",
                    ImagePath3="https://cdn.pixabay.com/photo/2014/10/22/17/25/stretching-498256__340.jpg",
                    ImagePath4="https://cdn.pixabay.com/photo/2014/10/22/17/25/stretching-498256__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(1),
                    Name="5 yards ankara",Price = 26,Quantity=10,Tags="ankara,african,female",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2013/12/03/11/14/color-222851__340.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2016/03/27/18/14/morocco-1283373__340.jpg",
                    ImagePath3="",
                    ImagePath4="",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(1),
                    Name="3 yards green ankara",Price = 22,Quantity=11,Tags="ankara,african,male",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://media.istockphoto.com/photos/mudcloth-picture-id115859522?b=1&k=6&m=115859522&s=170667a&w=0&h=iScevI-jJoS4mD3q4uQwmEKHwIuWJ6VTiXn0g2Qf-Oo=",
                    ImagePath2="",
                    ImagePath3="",
                    ImagePath4="",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(3),
                    Name="Baby blue and red overall",Price = 25,Quantity=19,Tags="baby,babywears",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://media.istockphoto.com/photos/blank-white-babygrow-bodysuit-surrounded-by-delicate-pretty-pink-on-picture-id1164197250?b=1&k=6&m=1164197250&s=170667a&w=0&h=y9OSBuUfEE3YBll8u-ppUtDYT159WZOKgoE8lafuzT0=",
                    ImagePath2="https://cdn.pixabay.com/photo/2016/02/13/14/52/doll-1197952__340.jpg",
                    ImagePath3="",
                    ImagePath4="https://cdn.pixabay.com/photo/2016/02/13/14/52/doll-1197952__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(3),
                    Name="Baby feeder",Price = 5,Quantity=10,Tags="baby",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2014/04/02/14/14/baby-306567__340.png",
                    ImagePath2="",
                    ImagePath3="https://media.istockphoto.com/photos/young-father-kiss-his-baby-during-drinking-milk-picture-id1142885375?b=1&k=6&m=1142885375&s=170667a&w=0&h=g0Yx-8d2nX989J6MPqkVFpznZu_ARJXMe14Hk4qxiX8=",
                    ImagePath4="https://media.istockphoto.com/photos/young-father-kiss-his-baby-during-drinking-milk-picture-id1142885375?b=1&k=6&m=1142885375&s=170667a&w=0&h=g0Yx-8d2nX989J6MPqkVFpznZu_ARJXMe14Hk4qxiX8=",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(1),
                    Name="2 yards blue lace",Price = 20,Quantity=10,Tags="lace,african",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2020/04/09/06/04/sveg-5019851__340.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2020/04/09/17/05/denim-jumpsuit-5022344__340.jpg",
                    ImagePath3="https://cdn.pixabay.com/photo/2020/04/09/17/05/denim-jumpsuit-5022344__340.jpg",
                    ImagePath4="https://cdn.pixabay.com/photo/2020/04/09/17/05/denim-jumpsuit-5022339__340.jpg",
                },

                 new Product
                {
                    IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,Category=context.Category.Find(1),
                    Name="2 yards red lace",Price = 19,Quantity=13,Tags="lace,african",
                    Description="Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    ImagePath1="https://cdn.pixabay.com/photo/2017/10/18/11/52/hat-2863883__340.jpg",
                    ImagePath2="https://cdn.pixabay.com/photo/2017/10/18/11/51/hat-2863879__340.jpg",
                    ImagePath3="https://cdn.pixabay.com/photo/2017/10/18/11/44/hat-2863836__340.jpg",
                    ImagePath4="",
                },

            };

                products.ForEach(p => context.Product.Add(p));
                context.SaveChanges();

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
