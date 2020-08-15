using DQueensFashion.Core.Model;
using DQueensFashion.CustomFilters;
using DQueensFashion.Models;
using DQueensFashion.Service.Contract;
using DQueensFashion.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DQueensFashion.Controllers
{
    [HomeSetGlobalVariable]
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IReviewService _reviewService;
        private readonly IImageService _imageService;
        private readonly ILineItemService _lineItemService;


        public HomeController(IProductService productService, ICategoryService categoryService, IReviewService reviewService,IImageService imageService,ILineItemService lineItemService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _reviewService = reviewService;
            _imageService = imageService;
            _lineItemService = lineItemService;
        }

        public ActionResult Index()
        {
            GeneralService generalService = new GeneralService();
            var allImages = _imageService.GetAllImageMainFiles().ToList();

            IEnumerable<ViewProductsViewModel> products = _productService.GetAllProducts()
                .Where(p=>p.ForSale)
                .Select(p => new ViewProductsViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    GeneratedUrl = generalService.GenerateItemNameAsParam(p.Id, p.Name),
                    MainImage = allImages.Where(image => image.ProductId == p.Id).Count() < 1 ?
                        AppConstant.DefaultProductImage :
                        allImages.Where(image => image.ProductId == p.Id).FirstOrDefault().ImagePath,
                    Price = p.Price,
                    Discount = p.Discount,
                    SubTotal = p.SubTotal,
                    Category = p.Category.Name,
                    CategoryId = p.Category.Id,
                    Rating = new RatingViewModel()
                    {
                        AverageRating = p.AverageRating.ToString("0.0"),
                        IsDouble = (p.AverageRating % 1) == 0 ? false : true,
                        FloorAverageRating = (int)Math.Floor(p.AverageRating)
                    },
                    NumberOfOrders = p.NumberOfItemsBought,
                    DateCreated = p.DateCreatedUtc,
                    IsNew = _productService.CheckIfProductIsNew(p.DateCreatedUtc),
                    IsOutOfStock = p.Quantity < 1 ? true : false,
                    LazyLoad = true,
                    ForSale = p.ForSale,
                }).OrderByDescending(p => p.DateCreated).ToList();

            //dont lazy load top images
            products.Take(4).ToList().ForEach(p => p.LazyLoad = false);

            List<ProductsByCategory> categorizedProducts = new List<ProductsByCategory>();

            IEnumerable<CategoryNameAndId> categories = _categoryService.GetAllCategories()
                .Select(category => new CategoryNameAndId()
                {
                    Id = category.Id,
                    Name = category.Name.ToUpper(),
                }).OrderBy(c => c.Name).ToList();

            foreach (var category in categories)
            {
                var _categorizedProduct = new ProductsByCategory()
                {
                    CategoryId = category.Id,
                    CategoryName = category.Name,
                    Products = products.Where(p => p.CategoryId == category.Id).Take(AppConstant.HomeIndexProductCount),
                };
                _categorizedProduct.Products.Take(4).ToList().ForEach(p => p.LazyLoad = false);
                categorizedProducts.Add(_categorizedProduct);
            }

            HomeIndexViewModel homeIndex = new HomeIndexViewModel()
            {
                Products = products.Take(AppConstant.HomeIndexProductCount),
                Categories = categories,
                BestSellingProducts = products.OrderByDescending(p => p.NumberOfOrders).Take(4).ToList(),
                BestDealsProducts = products.OrderByDescending(p => p.Discount).Take(8).ToList(),
                CategorizedProducts = categorizedProducts,
            };

            return View(homeIndex);
        }

        public ActionResult Test()
        {
            var categories = _categoryService.GetAllCategories();
            Random rand = new Random();
            string randomString = "Pellentesque a nisl sit amet augue vestibulum pretium. Class aptent taciti sociosqu ad litora torquent per conubia nostra per inceptos himenaeos. Nullam quis pellentesque erat. Donec semper ac arcu sit amet ornare. Vestibulum et dolor consequat lectus elementum pulvinar sit amet non nunc. Cras rutrum augue leo, eget convallis erat sodales et. Maecenas dignissim metus felis, ut euismod tellus consequat nec. Integer quis dapibus justo. Vivamus quis dolor eu ipsum sodales posuere. Interdum et malesuada fames ac ante ipsum primis in faucibus. Proin nec libero orci. Nulla facilisi. Aliquam et eleifend ex. Vestibulum aliquam pretium gravida.Etiam eget dolor non metus consequat mattis.Suspendisse semper sodales sapien a vehicula. Sed nec sodales ante. Maecenas imperdiet orci ac ligula scelerisque, in rutrum risus viverra.Nam a sapien semper, laoreet augue et, vestibulum ex.Etiam at ipsum et ex sodales suscipit at non enim. Cras in ipsum risus. Nulla molestie lacinia turpis. Vestibulum vel mauris est. Donec vulputate, arcu ut finibus tristique, eros leo luctus nisi, eget tempus urna nulla nec nibh.Etiam luctus commodo diam, eget vestibulum libero molestie sed. Duis vel laoreet orci, vel dictum arcu. Phasellus vehicula ligula nulla. In nunc arcu, hendrerit eu risus dignissim, varius mollis metus.Donec malesuada semper metus non eleifend. Morbi in rutrum nulla, non aliquam libero. Cras sodales arcu facilisis dolor tempor, vitae interdum nulla fermentum.Fusce vel tellus ullamcorper, interdum odio in, sollicitudin ipsum. Nullam in nunc eget mi venenatis egestas in eget dui. Nam iaculis lorem velit, non finibus urna aliquet ut. Donec dignissim, velit et tincidunt tincidunt, erat sapien aliquet augue, quis egestas augue nibh eu orci.Sed aliquet sem at nunc cursus lacinia.Quisque lacinia ipsum nec urna feugiat ornare.Duis ultricies suscipit lacus eu pretium. Duis vel risus nulla. Vestibulum in tellus ac lorem fringilla consectetur ac non felis. Ut convallis suscipit turpis, nec fermentum dui ultrices ac.";
            randomString = Regex.Replace(randomString, @"[^0-9a-zA-Z]+", "");

            List<Product> products = new List<Product>();
            for(int i = 0; i < 20; i++)
            {
                int nameStart = rand.Next(1, randomString.Length - 30);

                int randomCategoryId = rand.Next(1, categories.Count() + 1);
                products.Add(new Product()
                {
                    Name = randomString.Substring(nameStart, rand.Next(15, 30)),
                    Description = "Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                    Category = categories.Where(c => c.Id == randomCategoryId).FirstOrDefault(),
                    CategoryId = categories.Where(c => c.Id == randomCategoryId).FirstOrDefault().Id,
                    Price = rand.Next(3,21),
                    Discount = rand.Next(5,19),
                    Quantity = rand.Next(3,50),
                    Tags= "",
                    DeliveryDaysDuration= rand.Next(1,11),
                });
            }
            _productService.AddProductRange(products);

            int lowestProductId = products.Min(p => p.Id);
            int productCount = products.Max(p => p.Id);

            var images = new List<ImageFile>()
                {
                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2018/01/06/09/25/hijab-3064633__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2018/01/13/19/39/fashion-3080644__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2015/11/26/00/14/fashion-1063100__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2017/05/13/12/40/fashion-2309519__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2018/03/12/12/32/woman-3219507__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2015/01/15/13/06/model-600238__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2017/01/23/19/40/woman-2003647__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2017/06/02/14/11/girl-2366438__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2016/06/06/17/05/model-1439909__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2015/09/03/08/04/photographer-920128__340.jpg",
                        ProductId =rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2016/05/17/22/16/baby-1399332__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2014/05/21/14/54/feet-349687__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2016/11/16/10/28/two-girls-1828539__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2016/11/22/10/47/girl-1848947__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2015/10/12/14/59/girl-984060__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2016/11/23/17/24/automobile-1853936__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2016/11/23/17/24/automobile-1853936__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2016/11/29/11/32/beautiful-1869208__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2016/03/09/10/23/model-1246028__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2016/11/23/15/26/fashion-1853507__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2018/01/29/17/01/beautiful-3116587__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2015/07/28/09/18/dress-864107__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2016/11/29/10/10/girl-1868930__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2014/01/03/01/13/girl-237871__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2018/04/07/19/39/woman-3299379__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2017/10/19/18/23/actress-2868705__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2016/11/22/06/27/girl-1848473__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2016/11/29/07/16/balancing-1868051__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2016/07/18/10/16/model-1525629__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2016/11/29/09/25/beautiful-1868701__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},

                    new ImageFile{ IsDeleted=false, DateCreated=DateTime.Now, DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                        ImagePath ="https://cdn.pixabay.com/photo/2016/11/29/01/34/fashion-1866574__340.jpg",ProductId=rand.Next(lowestProductId,productCount)},
                };
            for (int i = 0; i < 3; i++)
            {
                _imageService.AddRangeImages(images);
            }

            foreach (var product in products)
            {
                var reviews = new List<Review>()
                    {
                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.",
                            Name = "James Victor",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "using Content here, content here, making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for lorem ipsum will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                            Name="Victor Daniel",
                            Email = "balam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old.",
                            Name = "Adekunle Gold",
                            Email = "AGbaby@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source.",
                            Name = "Olorunfemi John",
                            Email = "john@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. ",
                            Name = "Simi oyekunke",
                            Email = "simi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. book.",
                            Name = "Eniola",
                            Email = "Eny@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = " It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.",
                            Name = "Cornor",
                            Email = "cnor@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system",
                            Name = "Paul",
                            Email = "pp@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure.",
                            Name = "Opeyemi",
                            Email = "opy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "On the other hand, we denounce with righteous indignation and dislike men who are so beguiled and demoralized by the charms of pleasure of the moment, so blinded by desire, that they cannot foresee the pain and trouble that are bound to ensue; and equal blame belongs to those who fail in their duty through weakness of will, which is the same as saying through shrinking from toil and pain. These cases are perfectly simple and easy to distinguish. ",
                            Name = "KingBach",
                            Email = "king@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Mauris eu purus sed ipsum egestas ullamcorper. Aenean nec sem pretium velit lacinia varius at vel leo. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Aenean convallis ut nunc sit amet fermentum. Etiam non vehicula neque, iaculis luctus sem. Aenean efficitur, nibh a pellentesque rutrum, dui lorem sagittis ante, ut malesuada magna lacus quis dolor. Vestibulum feugiat commodo luctus",
                            Name = "Tresh",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Praesent fringilla egestas lacus, sit amet consequat neque ultrices ultrices. Etiam sit amet sem dolor. Praesent rhoncus tincidunt ex eget mollis. Cras maximus, enim eu tempor tristique, quam augue volutpat dolor, a volutpat nibh ipsum at sapien. Nam turpis arcu, elementum sed laoreet quis, commodo vel felis. Sed sed pretium felis, sed iaculis urna. Suspendisse scelerisque lacinia purus eget porta. Cras eget tempor lorem",
                            Name = "Femi",
                            Email = "femi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Curabitur ullamcorper ante sed porttitor maximus. Proin pharetra vitae nunc nec tristique. Curabitur aliquam tristique diam a aliquam. Nulla tellus diam, commodo non condimentum at, hendrerit eu diam. Curabitur facilisis nunc sit amet leo cursus, at tristique purus blandit.",
                            Name = "Victor",
                            Email = "victo@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Praesent fringilla egestas lacus, sit amet consequat neque ultrices ultrices. Etiam sit amet sem dolor. Praesent rhoncus tincidunt ex eget mollis. Cras maximus, enim eu tempor tristique, quam augue volutpat dolor, a volutpat nibh ipsum at sapien. Nam turpis arcu, elementum sed laoreet quis, commodo vel felis. Sed sed pretium felis, sed iaculis urna. Suspendisse scelerisque lacinia purus eget porta. Cras eget tempor lorem.",
                            Name = "Grizmann",
                            Email = "grizzy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Mauris mollis auctor velit, vitae scelerisque mauris feugiat sed. Mauris eget cursus tortor. Curabitur a dui in turpis consectetur gravida",
                            Name = "Sandra",
                            Email = "sandy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Aenean vel egestas dolor. Phasellus ac dictum tellus. Nam vel ante sit amet velit finibus finibus quis ut nisi. Mauris et varius est. Mauris vel efficitur odio. Duis faucibus nulla lacus, vel feugiat dui consectetur et.",
                            Name = "Saul goodman",
                            Email = "saulG@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Duis aliquet, enim lobortis ultricies euismod, risus neque tincidunt risus, in vestibulum erat nunc vitae nisl. In congue erat vel ultricies egestas. Vestibulum placerat, elit non condimentum maximus, sapien ligula porttitor orci, at pellentesque est velit non est. Ut efficitur ligula magna, id laoreet est faucibus non.",
                            Name = "Kendel",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = " Donec fermentum lacus et lacus pharetra, ut molestie justo semper. Curabitur id ultricies tortor. Fusce quis elit sit amet velit vulputate molestie. Vivamus at euismod ex. Etiam sit amet porttitor ligula.",
                            Name = "Jet Victor",
                            Email = "jv@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Duis aliquet, enim lobortis ultricies euismod, risus neque tincidunt risus, in vestibulum erat nunc vitae nisl. In congue erat vel ultricies egestas. Vestibulum placerat, elit non condimentum maximus, sapien ligula porttitor orci, at pellentesque est velit non est. Ut efficitur ligula magna, id laoreet est faucibus non.",
                            Name = "Walter white",
                            Email = "walter@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Aliquam euismod nisl imperdiet enim consectetur, eu dapibus massa placerat. Phasellus cursus vehicula mi, at tincidunt odio consectetur at. Integer neque turpis, sagittis vel ullamcorper imperdiet, ullamcorper quis dolor. Donec vitae rhoncus nunc. Maecenas placerat arcu in nisi viverra viverra.",
                            Name = "Dembele",
                            Email = "dd@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Nam tempus fringilla ligula, eget consequat dolor tincidunt id. Aliquam tincidunt, lectus ac hendrerit cursus, dui odio facilisis ligula, et placerat nisi leo quis nunc. Donec fermentum lacus et lacus pharetra, ut molestie justo semper. Curabitur id ultricies tortor. Fusce quis elit sit amet velit vulputate molestie. Vivamus at euismod ex. Etiam sit amet porttitor ligula.",
                            Name = "Suarez",
                            Email = "suarez@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Donec at ante a ante commodo consequat. Donec at cursus justo. Phasellus pretium elit sit amet enim placerat, non volutpat ligula sollicitudin. Curabitur a sem ac enim lacinia fringilla in nec nisl. Suspendisse potenti. Maecenas orci quam, eleifend eu metus at, placerat imperdiet elit. ",
                            Name = "Donald",
                            Email = "don@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Curabitur pellentesque elementum mauris, ac vulputate purus vehicula at. Pellentesque non tortor ornare, consectetur nisl et, hendrerit quam. Morbi mattis accumsan purus eget ornare. Donec interdum tristique nulla, rutrum posuere felis tincidunt et. Praesent suscipit et nulla nec scelerisque.",
                            Name = "Shayme",
                            Email = "sh@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = " Fusce vehicula lectus in ex aliquam sollicitudin. Nunc nec laoreet tellus. Mauris eu elit a nulla ultrices lobortis in condimentum mi. Curabitur porta nunc quis faucibus venenatis.",
                            Name = "torbido",
                            Email = "tr@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Phasellus eleifend pharetra urna non maximus. Nulla in pretium risus. Sed aliquet tempor felis ac faucibus. Nulla posuere elit nec erat elementum egestas. Aenean convallis ligula vulputate posuere egestas. Etiam vestibulum, lectus sit amet rhoncus facilisis, nunc arcu hendrerit neque, ac auctor eros velit ut nisi.",
                            Name = "Benzema",
                            Email = "bel@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Aenean id lobortis nulla. Phasellus pretium elit nec ullamcorper bibendum. Nullam bibendum erat a mi rhoncus iaculis eu sit amet mauris. Curabitur quam velit, sollicitudin at ultrices ut, mollis id augue. Nam dapibus facilisis sem. Nam malesuada nunc at est sollicitudin, a luctus sapien eleifend. Praesent mi purus",
                            Name = "Andrew",
                            Email = "andy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Mauris vestibulum dolor non est rutrum, quis consectetur quam pharetra. Donec a elementum sem. Quisque at nibh condimentum, fringilla diam et, volutpat magna. Sed consectetur vitae mauris et rhoncus. Etiam eget odio dolor. Sed vel justo nec est pellentesque volutpat eget eget enim",
                            Name = "Lumi",
                            Email = "lucas@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Duis at blandit purus. Suspendisse potenti. Nam ac ornare ante. Aliquam posuere ultricies turpis et laoreet. Nam eleifend magna et nulla ultricies, sit amet fringilla ante varius. Donec blandit massa quam, nec finibus turpis dapibus id. Integer aliquet malesuada turpis eget euismod. ",
                            Name = "Jogn cena",
                            Email = "cena@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Pellentesque malesuada, est nec egestas vulputate, velit mauris vehicula velit,.",
                            Name = "Cynthia",
                            Email = "cynrthia@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Vestibulum tristique non magna gravida blandit. Duis consequat sodales massa ut sollicitudin. Donec imperdiet congue lectus eu mattis. Mauris pharetra blandit tincidunt. Aliquam erat mi, faucibus at tortor ut, elementum dictum leo. Proin nec volutpat dui.",
                            Name = "Lionel messi",
                            Email = "lmessi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Etiam dictum nec lorem in dignissim. Nam et ex volutpat, ultrices augue vel, aliquam tortor. Nunc volutpat euismod tortor.",
                            Name = "Ethanla",
                            Email = "thye@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Morbi hendrerit lectus in magna mattis, sed scelerisque leo mattis. Nam blandit commodo turpis ut ornare. Donec non nisl ornare, ullamcorper massa quis, ultricies ipsum. Donec at sapien ac libero laoreet finibus. Sed ullamcorper nulla eu venenatis placerat. In sed lobortis eros. Nunc ut venenatis tortor",
                            Name = "Gabriel Victor",
                            Email = "gabriel@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                    };
                _reviewService.AddRangeReveiew(reviews);

            }

            products = _productService.GetAllProducts().ToList();
            foreach (var product in products)
            {
                product.AverageRating = _reviewService.GetAverageRating(product.Id);
                product.SubTotal = _productService.CalculateProductPrice(product.Price, product.Discount);
                _productService.UpdateProductNoVal(product);
            }

            ViewBag.Count = _productService.GetAllProductsCount();
            return View();
        }


        public ActionResult Det()
        {
            //var allProducuts = _productService.GetAllProductsWithDelete().ToList();
            //Random rand = new Random();
            //foreach (var product in allProducuts)
            //{
            //    product.ForSale = true;
            //    if (product.Quantity < 1)
            //        product.Quantity = rand.Next(1, 31);
            //    if (product.SubTotal < 1)
            //        product.SubTotal = _productService.CalculateProductPrice(product.Price, product.Discount);
            //    _productService.UpdateProductNoVal(product);
            //}
            //return Content(allProducuts.Where(p => p.ForSale == null).Count().ToString());
            return View();
        }

        public ActionResult Review1()
        {
            try
            {
                var products = _productService.GetAllProducts().Take(10);

                Random rand = new Random();

                foreach (var product in products)
                {
                    var reviews = new List<Review>()
                    {
                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.",
                            Name = "James Victor",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "using Content here, content here, making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for lorem ipsum will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                            Name="Victor Daniel",
                            Email = "balam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old.",
                            Name = "Adekunle Gold",
                            Email = "AGbaby@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source.",
                            Name = "Olorunfemi John",
                            Email = "john@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. ",
                            Name = "Simi oyekunke",
                            Email = "simi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. book.",
                            Name = "Eniola",
                            Email = "Eny@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = " It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.",
                            Name = "Cornor",
                            Email = "cnor@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system",
                            Name = "Paul",
                            Email = "pp@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure.",
                            Name = "Opeyemi",
                            Email = "opy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "On the other hand, we denounce with righteous indignation and dislike men who are so beguiled and demoralized by the charms of pleasure of the moment, so blinded by desire, that they cannot foresee the pain and trouble that are bound to ensue; and equal blame belongs to those who fail in their duty through weakness of will, which is the same as saying through shrinking from toil and pain. These cases are perfectly simple and easy to distinguish. ",
                            Name = "KingBach",
                            Email = "king@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Mauris eu purus sed ipsum egestas ullamcorper. Aenean nec sem pretium velit lacinia varius at vel leo. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Aenean convallis ut nunc sit amet fermentum. Etiam non vehicula neque, iaculis luctus sem. Aenean efficitur, nibh a pellentesque rutrum, dui lorem sagittis ante, ut malesuada magna lacus quis dolor. Vestibulum feugiat commodo luctus",
                            Name = "Tresh",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Praesent fringilla egestas lacus, sit amet consequat neque ultrices ultrices. Etiam sit amet sem dolor. Praesent rhoncus tincidunt ex eget mollis. Cras maximus, enim eu tempor tristique, quam augue volutpat dolor, a volutpat nibh ipsum at sapien. Nam turpis arcu, elementum sed laoreet quis, commodo vel felis. Sed sed pretium felis, sed iaculis urna. Suspendisse scelerisque lacinia purus eget porta. Cras eget tempor lorem",
                            Name = "Femi",
                            Email = "femi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Curabitur ullamcorper ante sed porttitor maximus. Proin pharetra vitae nunc nec tristique. Curabitur aliquam tristique diam a aliquam. Nulla tellus diam, commodo non condimentum at, hendrerit eu diam. Curabitur facilisis nunc sit amet leo cursus, at tristique purus blandit.",
                            Name = "Victor",
                            Email = "victo@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Praesent fringilla egestas lacus, sit amet consequat neque ultrices ultrices. Etiam sit amet sem dolor. Praesent rhoncus tincidunt ex eget mollis. Cras maximus, enim eu tempor tristique, quam augue volutpat dolor, a volutpat nibh ipsum at sapien. Nam turpis arcu, elementum sed laoreet quis, commodo vel felis. Sed sed pretium felis, sed iaculis urna. Suspendisse scelerisque lacinia purus eget porta. Cras eget tempor lorem.",
                            Name = "Grizmann",
                            Email = "grizzy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Mauris mollis auctor velit, vitae scelerisque mauris feugiat sed. Mauris eget cursus tortor. Curabitur a dui in turpis consectetur gravida",
                            Name = "Sandra",
                            Email = "sandy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Aenean vel egestas dolor. Phasellus ac dictum tellus. Nam vel ante sit amet velit finibus finibus quis ut nisi. Mauris et varius est. Mauris vel efficitur odio. Duis faucibus nulla lacus, vel feugiat dui consectetur et.",
                            Name = "Saul goodman",
                            Email = "saulG@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Duis aliquet, enim lobortis ultricies euismod, risus neque tincidunt risus, in vestibulum erat nunc vitae nisl. In congue erat vel ultricies egestas. Vestibulum placerat, elit non condimentum maximus, sapien ligula porttitor orci, at pellentesque est velit non est. Ut efficitur ligula magna, id laoreet est faucibus non.",
                            Name = "Kendel",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = " Donec fermentum lacus et lacus pharetra, ut molestie justo semper. Curabitur id ultricies tortor. Fusce quis elit sit amet velit vulputate molestie. Vivamus at euismod ex. Etiam sit amet porttitor ligula.",
                            Name = "Jet Victor",
                            Email = "jv@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Duis aliquet, enim lobortis ultricies euismod, risus neque tincidunt risus, in vestibulum erat nunc vitae nisl. In congue erat vel ultricies egestas. Vestibulum placerat, elit non condimentum maximus, sapien ligula porttitor orci, at pellentesque est velit non est. Ut efficitur ligula magna, id laoreet est faucibus non.",
                            Name = "Walter white",
                            Email = "walter@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Aliquam euismod nisl imperdiet enim consectetur, eu dapibus massa placerat. Phasellus cursus vehicula mi, at tincidunt odio consectetur at. Integer neque turpis, sagittis vel ullamcorper imperdiet, ullamcorper quis dolor. Donec vitae rhoncus nunc. Maecenas placerat arcu in nisi viverra viverra.",
                            Name = "Dembele",
                            Email = "dd@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Nam tempus fringilla ligula, eget consequat dolor tincidunt id. Aliquam tincidunt, lectus ac hendrerit cursus, dui odio facilisis ligula, et placerat nisi leo quis nunc. Donec fermentum lacus et lacus pharetra, ut molestie justo semper. Curabitur id ultricies tortor. Fusce quis elit sit amet velit vulputate molestie. Vivamus at euismod ex. Etiam sit amet porttitor ligula.",
                            Name = "Suarez",
                            Email = "suarez@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Donec at ante a ante commodo consequat. Donec at cursus justo. Phasellus pretium elit sit amet enim placerat, non volutpat ligula sollicitudin. Curabitur a sem ac enim lacinia fringilla in nec nisl. Suspendisse potenti. Maecenas orci quam, eleifend eu metus at, placerat imperdiet elit. ",
                            Name = "Donald",
                            Email = "don@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Curabitur pellentesque elementum mauris, ac vulputate purus vehicula at. Pellentesque non tortor ornare, consectetur nisl et, hendrerit quam. Morbi mattis accumsan purus eget ornare. Donec interdum tristique nulla, rutrum posuere felis tincidunt et. Praesent suscipit et nulla nec scelerisque.",
                            Name = "Shayme",
                            Email = "sh@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = " Fusce vehicula lectus in ex aliquam sollicitudin. Nunc nec laoreet tellus. Mauris eu elit a nulla ultrices lobortis in condimentum mi. Curabitur porta nunc quis faucibus venenatis.",
                            Name = "torbido",
                            Email = "tr@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Phasellus eleifend pharetra urna non maximus. Nulla in pretium risus. Sed aliquet tempor felis ac faucibus. Nulla posuere elit nec erat elementum egestas. Aenean convallis ligula vulputate posuere egestas. Etiam vestibulum, lectus sit amet rhoncus facilisis, nunc arcu hendrerit neque, ac auctor eros velit ut nisi.",
                            Name = "Benzema",
                            Email = "bel@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Aenean id lobortis nulla. Phasellus pretium elit nec ullamcorper bibendum. Nullam bibendum erat a mi rhoncus iaculis eu sit amet mauris. Curabitur quam velit, sollicitudin at ultrices ut, mollis id augue. Nam dapibus facilisis sem. Nam malesuada nunc at est sollicitudin, a luctus sapien eleifend. Praesent mi purus",
                            Name = "Andrew",
                            Email = "andy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Mauris vestibulum dolor non est rutrum, quis consectetur quam pharetra. Donec a elementum sem. Quisque at nibh condimentum, fringilla diam et, volutpat magna. Sed consectetur vitae mauris et rhoncus. Etiam eget odio dolor. Sed vel justo nec est pellentesque volutpat eget eget enim",
                            Name = "Lumi",
                            Email = "lucas@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Duis at blandit purus. Suspendisse potenti. Nam ac ornare ante. Aliquam posuere ultricies turpis et laoreet. Nam eleifend magna et nulla ultricies, sit amet fringilla ante varius. Donec blandit massa quam, nec finibus turpis dapibus id. Integer aliquet malesuada turpis eget euismod. ",
                            Name = "Jogn cena",
                            Email = "cena@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Pellentesque malesuada, est nec egestas vulputate, velit mauris vehicula velit,.",
                            Name = "Cynthia",
                            Email = "cynrthia@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Vestibulum tristique non magna gravida blandit. Duis consequat sodales massa ut sollicitudin. Donec imperdiet congue lectus eu mattis. Mauris pharetra blandit tincidunt. Aliquam erat mi, faucibus at tortor ut, elementum dictum leo. Proin nec volutpat dui.",
                            Name = "Lionel messi",
                            Email = "lmessi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Etiam dictum nec lorem in dignissim. Nam et ex volutpat, ultrices augue vel, aliquam tortor. Nunc volutpat euismod tortor.",
                            Name = "Ethanla",
                            Email = "thye@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Morbi hendrerit lectus in magna mattis, sed scelerisque leo mattis. Nam blandit commodo turpis ut ornare. Donec non nisl ornare, ullamcorper massa quis, ultricies ipsum. Donec at sapien ac libero laoreet finibus. Sed ullamcorper nulla eu venenatis placerat. In sed lobortis eros. Nunc ut venenatis tortor",
                            Name = "Gabriel Victor",
                            Email = "gabriel@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },



                    };
                    _reviewService.AddRangeReveiew(reviews);

                }
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult Review2()
        {
            var products = _productService.GetAllProducts().Skip(10).Take(10);

            Random rand = new Random();

            foreach (var product in products)
            {
                var reviews = new List<Review>()
                    {
                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.",
                            Name = "James Victor",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "using Content here, content here, making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for lorem ipsum will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                            Name="Victor Daniel",
                            Email = "balam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old.",
                            Name = "Adekunle Gold",
                            Email = "AGbaby@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source.",
                            Name = "Olorunfemi John",
                            Email = "john@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. ",
                            Name = "Simi oyekunke",
                            Email = "simi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. book.",
                            Name = "Eniola",
                            Email = "Eny@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = " It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.",
                            Name = "Cornor",
                            Email = "cnor@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system",
                            Name = "Paul",
                            Email = "pp@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure.",
                            Name = "Opeyemi",
                            Email = "opy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "On the other hand, we denounce with righteous indignation and dislike men who are so beguiled and demoralized by the charms of pleasure of the moment, so blinded by desire, that they cannot foresee the pain and trouble that are bound to ensue; and equal blame belongs to those who fail in their duty through weakness of will, which is the same as saying through shrinking from toil and pain. These cases are perfectly simple and easy to distinguish. ",
                            Name = "KingBach",
                            Email = "king@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Mauris eu purus sed ipsum egestas ullamcorper. Aenean nec sem pretium velit lacinia varius at vel leo. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Aenean convallis ut nunc sit amet fermentum. Etiam non vehicula neque, iaculis luctus sem. Aenean efficitur, nibh a pellentesque rutrum, dui lorem sagittis ante, ut malesuada magna lacus quis dolor. Vestibulum feugiat commodo luctus",
                            Name = "Tresh",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Praesent fringilla egestas lacus, sit amet consequat neque ultrices ultrices. Etiam sit amet sem dolor. Praesent rhoncus tincidunt ex eget mollis. Cras maximus, enim eu tempor tristique, quam augue volutpat dolor, a volutpat nibh ipsum at sapien. Nam turpis arcu, elementum sed laoreet quis, commodo vel felis. Sed sed pretium felis, sed iaculis urna. Suspendisse scelerisque lacinia purus eget porta. Cras eget tempor lorem",
                            Name = "Femi",
                            Email = "femi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Curabitur ullamcorper ante sed porttitor maximus. Proin pharetra vitae nunc nec tristique. Curabitur aliquam tristique diam a aliquam. Nulla tellus diam, commodo non condimentum at, hendrerit eu diam. Curabitur facilisis nunc sit amet leo cursus, at tristique purus blandit.",
                            Name = "Victor",
                            Email = "victo@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Praesent fringilla egestas lacus, sit amet consequat neque ultrices ultrices. Etiam sit amet sem dolor. Praesent rhoncus tincidunt ex eget mollis. Cras maximus, enim eu tempor tristique, quam augue volutpat dolor, a volutpat nibh ipsum at sapien. Nam turpis arcu, elementum sed laoreet quis, commodo vel felis. Sed sed pretium felis, sed iaculis urna. Suspendisse scelerisque lacinia purus eget porta. Cras eget tempor lorem.",
                            Name = "Grizmann",
                            Email = "grizzy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Mauris mollis auctor velit, vitae scelerisque mauris feugiat sed. Mauris eget cursus tortor. Curabitur a dui in turpis consectetur gravida",
                            Name = "Sandra",
                            Email = "sandy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Aenean vel egestas dolor. Phasellus ac dictum tellus. Nam vel ante sit amet velit finibus finibus quis ut nisi. Mauris et varius est. Mauris vel efficitur odio. Duis faucibus nulla lacus, vel feugiat dui consectetur et.",
                            Name = "Saul goodman",
                            Email = "saulG@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Duis aliquet, enim lobortis ultricies euismod, risus neque tincidunt risus, in vestibulum erat nunc vitae nisl. In congue erat vel ultricies egestas. Vestibulum placerat, elit non condimentum maximus, sapien ligula porttitor orci, at pellentesque est velit non est. Ut efficitur ligula magna, id laoreet est faucibus non.",
                            Name = "Kendel",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = " Donec fermentum lacus et lacus pharetra, ut molestie justo semper. Curabitur id ultricies tortor. Fusce quis elit sit amet velit vulputate molestie. Vivamus at euismod ex. Etiam sit amet porttitor ligula.",
                            Name = "Jet Victor",
                            Email = "jv@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Duis aliquet, enim lobortis ultricies euismod, risus neque tincidunt risus, in vestibulum erat nunc vitae nisl. In congue erat vel ultricies egestas. Vestibulum placerat, elit non condimentum maximus, sapien ligula porttitor orci, at pellentesque est velit non est. Ut efficitur ligula magna, id laoreet est faucibus non.",
                            Name = "Walter white",
                            Email = "walter@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Aliquam euismod nisl imperdiet enim consectetur, eu dapibus massa placerat. Phasellus cursus vehicula mi, at tincidunt odio consectetur at. Integer neque turpis, sagittis vel ullamcorper imperdiet, ullamcorper quis dolor. Donec vitae rhoncus nunc. Maecenas placerat arcu in nisi viverra viverra.",
                            Name = "Dembele",
                            Email = "dd@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Nam tempus fringilla ligula, eget consequat dolor tincidunt id. Aliquam tincidunt, lectus ac hendrerit cursus, dui odio facilisis ligula, et placerat nisi leo quis nunc. Donec fermentum lacus et lacus pharetra, ut molestie justo semper. Curabitur id ultricies tortor. Fusce quis elit sit amet velit vulputate molestie. Vivamus at euismod ex. Etiam sit amet porttitor ligula.",
                            Name = "Suarez",
                            Email = "suarez@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Donec at ante a ante commodo consequat. Donec at cursus justo. Phasellus pretium elit sit amet enim placerat, non volutpat ligula sollicitudin. Curabitur a sem ac enim lacinia fringilla in nec nisl. Suspendisse potenti. Maecenas orci quam, eleifend eu metus at, placerat imperdiet elit. ",
                            Name = "Donald",
                            Email = "don@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Curabitur pellentesque elementum mauris, ac vulputate purus vehicula at. Pellentesque non tortor ornare, consectetur nisl et, hendrerit quam. Morbi mattis accumsan purus eget ornare. Donec interdum tristique nulla, rutrum posuere felis tincidunt et. Praesent suscipit et nulla nec scelerisque.",
                            Name = "Shayme",
                            Email = "sh@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = " Fusce vehicula lectus in ex aliquam sollicitudin. Nunc nec laoreet tellus. Mauris eu elit a nulla ultrices lobortis in condimentum mi. Curabitur porta nunc quis faucibus venenatis.",
                            Name = "torbido",
                            Email = "tr@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Phasellus eleifend pharetra urna non maximus. Nulla in pretium risus. Sed aliquet tempor felis ac faucibus. Nulla posuere elit nec erat elementum egestas. Aenean convallis ligula vulputate posuere egestas. Etiam vestibulum, lectus sit amet rhoncus facilisis, nunc arcu hendrerit neque, ac auctor eros velit ut nisi.",
                            Name = "Benzema",
                            Email = "bel@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Aenean id lobortis nulla. Phasellus pretium elit nec ullamcorper bibendum. Nullam bibendum erat a mi rhoncus iaculis eu sit amet mauris. Curabitur quam velit, sollicitudin at ultrices ut, mollis id augue. Nam dapibus facilisis sem. Nam malesuada nunc at est sollicitudin, a luctus sapien eleifend. Praesent mi purus",
                            Name = "Andrew",
                            Email = "andy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Mauris vestibulum dolor non est rutrum, quis consectetur quam pharetra. Donec a elementum sem. Quisque at nibh condimentum, fringilla diam et, volutpat magna. Sed consectetur vitae mauris et rhoncus. Etiam eget odio dolor. Sed vel justo nec est pellentesque volutpat eget eget enim",
                            Name = "Lumi",
                            Email = "lucas@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Duis at blandit purus. Suspendisse potenti. Nam ac ornare ante. Aliquam posuere ultricies turpis et laoreet. Nam eleifend magna et nulla ultricies, sit amet fringilla ante varius. Donec blandit massa quam, nec finibus turpis dapibus id. Integer aliquet malesuada turpis eget euismod. ",
                            Name = "Jogn cena",
                            Email = "cena@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Pellentesque malesuada, est nec egestas vulputate, velit mauris vehicula velit,.",
                            Name = "Cynthia",
                            Email = "cynrthia@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Vestibulum tristique non magna gravida blandit. Duis consequat sodales massa ut sollicitudin. Donec imperdiet congue lectus eu mattis. Mauris pharetra blandit tincidunt. Aliquam erat mi, faucibus at tortor ut, elementum dictum leo. Proin nec volutpat dui.",
                            Name = "Lionel messi",
                            Email = "lmessi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Etiam dictum nec lorem in dignissim. Nam et ex volutpat, ultrices augue vel, aliquam tortor. Nunc volutpat euismod tortor.",
                            Name = "Ethanla",
                            Email = "thye@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Morbi hendrerit lectus in magna mattis, sed scelerisque leo mattis. Nam blandit commodo turpis ut ornare. Donec non nisl ornare, ullamcorper massa quis, ultricies ipsum. Donec at sapien ac libero laoreet finibus. Sed ullamcorper nulla eu venenatis placerat. In sed lobortis eros. Nunc ut venenatis tortor",
                            Name = "Gabriel Victor",
                            Email = "gabriel@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },



                    };
                _reviewService.AddRangeReveiew(reviews);
            }
            return View();
        }


        public ActionResult Review3()
        {
            var products = _productService.GetAllProducts().Skip(20).Take(10);

            Random rand = new Random();

            foreach (var product in products)
            {
                var reviews = new List<Review>()
                    {
                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.",
                            Name = "James Victor",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "using Content here, content here, making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for lorem ipsum will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                            Name="Victor Daniel",
                            Email = "balam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old.",
                            Name = "Adekunle Gold",
                            Email = "AGbaby@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source.",
                            Name = "Olorunfemi John",
                            Email = "john@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. ",
                            Name = "Simi oyekunke",
                            Email = "simi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. book.",
                            Name = "Eniola",
                            Email = "Eny@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = " It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.",
                            Name = "Cornor",
                            Email = "cnor@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system",
                            Name = "Paul",
                            Email = "pp@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure.",
                            Name = "Opeyemi",
                            Email = "opy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "On the other hand, we denounce with righteous indignation and dislike men who are so beguiled and demoralized by the charms of pleasure of the moment, so blinded by desire, that they cannot foresee the pain and trouble that are bound to ensue; and equal blame belongs to those who fail in their duty through weakness of will, which is the same as saying through shrinking from toil and pain. These cases are perfectly simple and easy to distinguish. ",
                            Name = "KingBach",
                            Email = "king@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Mauris eu purus sed ipsum egestas ullamcorper. Aenean nec sem pretium velit lacinia varius at vel leo. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Aenean convallis ut nunc sit amet fermentum. Etiam non vehicula neque, iaculis luctus sem. Aenean efficitur, nibh a pellentesque rutrum, dui lorem sagittis ante, ut malesuada magna lacus quis dolor. Vestibulum feugiat commodo luctus",
                            Name = "Tresh",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Praesent fringilla egestas lacus, sit amet consequat neque ultrices ultrices. Etiam sit amet sem dolor. Praesent rhoncus tincidunt ex eget mollis. Cras maximus, enim eu tempor tristique, quam augue volutpat dolor, a volutpat nibh ipsum at sapien. Nam turpis arcu, elementum sed laoreet quis, commodo vel felis. Sed sed pretium felis, sed iaculis urna. Suspendisse scelerisque lacinia purus eget porta. Cras eget tempor lorem",
                            Name = "Femi",
                            Email = "femi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Curabitur ullamcorper ante sed porttitor maximus. Proin pharetra vitae nunc nec tristique. Curabitur aliquam tristique diam a aliquam. Nulla tellus diam, commodo non condimentum at, hendrerit eu diam. Curabitur facilisis nunc sit amet leo cursus, at tristique purus blandit.",
                            Name = "Victor",
                            Email = "victo@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Praesent fringilla egestas lacus, sit amet consequat neque ultrices ultrices. Etiam sit amet sem dolor. Praesent rhoncus tincidunt ex eget mollis. Cras maximus, enim eu tempor tristique, quam augue volutpat dolor, a volutpat nibh ipsum at sapien. Nam turpis arcu, elementum sed laoreet quis, commodo vel felis. Sed sed pretium felis, sed iaculis urna. Suspendisse scelerisque lacinia purus eget porta. Cras eget tempor lorem.",
                            Name = "Grizmann",
                            Email = "grizzy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Mauris mollis auctor velit, vitae scelerisque mauris feugiat sed. Mauris eget cursus tortor. Curabitur a dui in turpis consectetur gravida",
                            Name = "Sandra",
                            Email = "sandy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Aenean vel egestas dolor. Phasellus ac dictum tellus. Nam vel ante sit amet velit finibus finibus quis ut nisi. Mauris et varius est. Mauris vel efficitur odio. Duis faucibus nulla lacus, vel feugiat dui consectetur et.",
                            Name = "Saul goodman",
                            Email = "saulG@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Duis aliquet, enim lobortis ultricies euismod, risus neque tincidunt risus, in vestibulum erat nunc vitae nisl. In congue erat vel ultricies egestas. Vestibulum placerat, elit non condimentum maximus, sapien ligula porttitor orci, at pellentesque est velit non est. Ut efficitur ligula magna, id laoreet est faucibus non.",
                            Name = "Kendel",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = " Donec fermentum lacus et lacus pharetra, ut molestie justo semper. Curabitur id ultricies tortor. Fusce quis elit sit amet velit vulputate molestie. Vivamus at euismod ex. Etiam sit amet porttitor ligula.",
                            Name = "Jet Victor",
                            Email = "jv@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Duis aliquet, enim lobortis ultricies euismod, risus neque tincidunt risus, in vestibulum erat nunc vitae nisl. In congue erat vel ultricies egestas. Vestibulum placerat, elit non condimentum maximus, sapien ligula porttitor orci, at pellentesque est velit non est. Ut efficitur ligula magna, id laoreet est faucibus non.",
                            Name = "Walter white",
                            Email = "walter@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Aliquam euismod nisl imperdiet enim consectetur, eu dapibus massa placerat. Phasellus cursus vehicula mi, at tincidunt odio consectetur at. Integer neque turpis, sagittis vel ullamcorper imperdiet, ullamcorper quis dolor. Donec vitae rhoncus nunc. Maecenas placerat arcu in nisi viverra viverra.",
                            Name = "Dembele",
                            Email = "dd@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Nam tempus fringilla ligula, eget consequat dolor tincidunt id. Aliquam tincidunt, lectus ac hendrerit cursus, dui odio facilisis ligula, et placerat nisi leo quis nunc. Donec fermentum lacus et lacus pharetra, ut molestie justo semper. Curabitur id ultricies tortor. Fusce quis elit sit amet velit vulputate molestie. Vivamus at euismod ex. Etiam sit amet porttitor ligula.",
                            Name = "Suarez",
                            Email = "suarez@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Donec at ante a ante commodo consequat. Donec at cursus justo. Phasellus pretium elit sit amet enim placerat, non volutpat ligula sollicitudin. Curabitur a sem ac enim lacinia fringilla in nec nisl. Suspendisse potenti. Maecenas orci quam, eleifend eu metus at, placerat imperdiet elit. ",
                            Name = "Donald",
                            Email = "don@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Curabitur pellentesque elementum mauris, ac vulputate purus vehicula at. Pellentesque non tortor ornare, consectetur nisl et, hendrerit quam. Morbi mattis accumsan purus eget ornare. Donec interdum tristique nulla, rutrum posuere felis tincidunt et. Praesent suscipit et nulla nec scelerisque.",
                            Name = "Shayme",
                            Email = "sh@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = " Fusce vehicula lectus in ex aliquam sollicitudin. Nunc nec laoreet tellus. Mauris eu elit a nulla ultrices lobortis in condimentum mi. Curabitur porta nunc quis faucibus venenatis.",
                            Name = "torbido",
                            Email = "tr@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Phasellus eleifend pharetra urna non maximus. Nulla in pretium risus. Sed aliquet tempor felis ac faucibus. Nulla posuere elit nec erat elementum egestas. Aenean convallis ligula vulputate posuere egestas. Etiam vestibulum, lectus sit amet rhoncus facilisis, nunc arcu hendrerit neque, ac auctor eros velit ut nisi.",
                            Name = "Benzema",
                            Email = "bel@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Aenean id lobortis nulla. Phasellus pretium elit nec ullamcorper bibendum. Nullam bibendum erat a mi rhoncus iaculis eu sit amet mauris. Curabitur quam velit, sollicitudin at ultrices ut, mollis id augue. Nam dapibus facilisis sem. Nam malesuada nunc at est sollicitudin, a luctus sapien eleifend. Praesent mi purus",
                            Name = "Andrew",
                            Email = "andy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Mauris vestibulum dolor non est rutrum, quis consectetur quam pharetra. Donec a elementum sem. Quisque at nibh condimentum, fringilla diam et, volutpat magna. Sed consectetur vitae mauris et rhoncus. Etiam eget odio dolor. Sed vel justo nec est pellentesque volutpat eget eget enim",
                            Name = "Lumi",
                            Email = "lucas@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Duis at blandit purus. Suspendisse potenti. Nam ac ornare ante. Aliquam posuere ultricies turpis et laoreet. Nam eleifend magna et nulla ultricies, sit amet fringilla ante varius. Donec blandit massa quam, nec finibus turpis dapibus id. Integer aliquet malesuada turpis eget euismod. ",
                            Name = "Jogn cena",
                            Email = "cena@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Pellentesque malesuada, est nec egestas vulputate, velit mauris vehicula velit,.",
                            Name = "Cynthia",
                            Email = "cynrthia@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Vestibulum tristique non magna gravida blandit. Duis consequat sodales massa ut sollicitudin. Donec imperdiet congue lectus eu mattis. Mauris pharetra blandit tincidunt. Aliquam erat mi, faucibus at tortor ut, elementum dictum leo. Proin nec volutpat dui.",
                            Name = "Lionel messi",
                            Email = "lmessi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Etiam dictum nec lorem in dignissim. Nam et ex volutpat, ultrices augue vel, aliquam tortor. Nunc volutpat euismod tortor.",
                            Name = "Ethanla",
                            Email = "thye@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Morbi hendrerit lectus in magna mattis, sed scelerisque leo mattis. Nam blandit commodo turpis ut ornare. Donec non nisl ornare, ullamcorper massa quis, ultricies ipsum. Donec at sapien ac libero laoreet finibus. Sed ullamcorper nulla eu venenatis placerat. In sed lobortis eros. Nunc ut venenatis tortor",
                            Name = "Gabriel Victor",
                            Email = "gabriel@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },



                    };
                _reviewService.AddRangeReveiew(reviews);

            }
            return View();
        }


        public ActionResult Review4()
        {
            var products = _productService.GetAllProducts().Skip(30).Take(10);

            Random rand = new Random();

            foreach (var product in products)
            {
                var reviews = new List<Review>()
                    {
                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.",
                            Name = "James Victor",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "using Content here, content here, making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for lorem ipsum will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                            Name="Victor Daniel",
                            Email = "balam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old.",
                            Name = "Adekunle Gold",
                            Email = "AGbaby@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source.",
                            Name = "Olorunfemi John",
                            Email = "john@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. ",
                            Name = "Simi oyekunke",
                            Email = "simi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. book.",
                            Name = "Eniola",
                            Email = "Eny@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = " It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.",
                            Name = "Cornor",
                            Email = "cnor@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system",
                            Name = "Paul",
                            Email = "pp@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure.",
                            Name = "Opeyemi",
                            Email = "opy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "On the other hand, we denounce with righteous indignation and dislike men who are so beguiled and demoralized by the charms of pleasure of the moment, so blinded by desire, that they cannot foresee the pain and trouble that are bound to ensue; and equal blame belongs to those who fail in their duty through weakness of will, which is the same as saying through shrinking from toil and pain. These cases are perfectly simple and easy to distinguish. ",
                            Name = "KingBach",
                            Email = "king@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Mauris eu purus sed ipsum egestas ullamcorper. Aenean nec sem pretium velit lacinia varius at vel leo. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Aenean convallis ut nunc sit amet fermentum. Etiam non vehicula neque, iaculis luctus sem. Aenean efficitur, nibh a pellentesque rutrum, dui lorem sagittis ante, ut malesuada magna lacus quis dolor. Vestibulum feugiat commodo luctus",
                            Name = "Tresh",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Praesent fringilla egestas lacus, sit amet consequat neque ultrices ultrices. Etiam sit amet sem dolor. Praesent rhoncus tincidunt ex eget mollis. Cras maximus, enim eu tempor tristique, quam augue volutpat dolor, a volutpat nibh ipsum at sapien. Nam turpis arcu, elementum sed laoreet quis, commodo vel felis. Sed sed pretium felis, sed iaculis urna. Suspendisse scelerisque lacinia purus eget porta. Cras eget tempor lorem",
                            Name = "Femi",
                            Email = "femi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Curabitur ullamcorper ante sed porttitor maximus. Proin pharetra vitae nunc nec tristique. Curabitur aliquam tristique diam a aliquam. Nulla tellus diam, commodo non condimentum at, hendrerit eu diam. Curabitur facilisis nunc sit amet leo cursus, at tristique purus blandit.",
                            Name = "Victor",
                            Email = "victo@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Praesent fringilla egestas lacus, sit amet consequat neque ultrices ultrices. Etiam sit amet sem dolor. Praesent rhoncus tincidunt ex eget mollis. Cras maximus, enim eu tempor tristique, quam augue volutpat dolor, a volutpat nibh ipsum at sapien. Nam turpis arcu, elementum sed laoreet quis, commodo vel felis. Sed sed pretium felis, sed iaculis urna. Suspendisse scelerisque lacinia purus eget porta. Cras eget tempor lorem.",
                            Name = "Grizmann",
                            Email = "grizzy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Mauris mollis auctor velit, vitae scelerisque mauris feugiat sed. Mauris eget cursus tortor. Curabitur a dui in turpis consectetur gravida",
                            Name = "Sandra",
                            Email = "sandy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Aenean vel egestas dolor. Phasellus ac dictum tellus. Nam vel ante sit amet velit finibus finibus quis ut nisi. Mauris et varius est. Mauris vel efficitur odio. Duis faucibus nulla lacus, vel feugiat dui consectetur et.",
                            Name = "Saul goodman",
                            Email = "saulG@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Duis aliquet, enim lobortis ultricies euismod, risus neque tincidunt risus, in vestibulum erat nunc vitae nisl. In congue erat vel ultricies egestas. Vestibulum placerat, elit non condimentum maximus, sapien ligula porttitor orci, at pellentesque est velit non est. Ut efficitur ligula magna, id laoreet est faucibus non.",
                            Name = "Kendel",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = " Donec fermentum lacus et lacus pharetra, ut molestie justo semper. Curabitur id ultricies tortor. Fusce quis elit sit amet velit vulputate molestie. Vivamus at euismod ex. Etiam sit amet porttitor ligula.",
                            Name = "Jet Victor",
                            Email = "jv@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Duis aliquet, enim lobortis ultricies euismod, risus neque tincidunt risus, in vestibulum erat nunc vitae nisl. In congue erat vel ultricies egestas. Vestibulum placerat, elit non condimentum maximus, sapien ligula porttitor orci, at pellentesque est velit non est. Ut efficitur ligula magna, id laoreet est faucibus non.",
                            Name = "Walter white",
                            Email = "walter@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Aliquam euismod nisl imperdiet enim consectetur, eu dapibus massa placerat. Phasellus cursus vehicula mi, at tincidunt odio consectetur at. Integer neque turpis, sagittis vel ullamcorper imperdiet, ullamcorper quis dolor. Donec vitae rhoncus nunc. Maecenas placerat arcu in nisi viverra viverra.",
                            Name = "Dembele",
                            Email = "dd@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Nam tempus fringilla ligula, eget consequat dolor tincidunt id. Aliquam tincidunt, lectus ac hendrerit cursus, dui odio facilisis ligula, et placerat nisi leo quis nunc. Donec fermentum lacus et lacus pharetra, ut molestie justo semper. Curabitur id ultricies tortor. Fusce quis elit sit amet velit vulputate molestie. Vivamus at euismod ex. Etiam sit amet porttitor ligula.",
                            Name = "Suarez",
                            Email = "suarez@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Donec at ante a ante commodo consequat. Donec at cursus justo. Phasellus pretium elit sit amet enim placerat, non volutpat ligula sollicitudin. Curabitur a sem ac enim lacinia fringilla in nec nisl. Suspendisse potenti. Maecenas orci quam, eleifend eu metus at, placerat imperdiet elit. ",
                            Name = "Donald",
                            Email = "don@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Curabitur pellentesque elementum mauris, ac vulputate purus vehicula at. Pellentesque non tortor ornare, consectetur nisl et, hendrerit quam. Morbi mattis accumsan purus eget ornare. Donec interdum tristique nulla, rutrum posuere felis tincidunt et. Praesent suscipit et nulla nec scelerisque.",
                            Name = "Shayme",
                            Email = "sh@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = " Fusce vehicula lectus in ex aliquam sollicitudin. Nunc nec laoreet tellus. Mauris eu elit a nulla ultrices lobortis in condimentum mi. Curabitur porta nunc quis faucibus venenatis.",
                            Name = "torbido",
                            Email = "tr@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Phasellus eleifend pharetra urna non maximus. Nulla in pretium risus. Sed aliquet tempor felis ac faucibus. Nulla posuere elit nec erat elementum egestas. Aenean convallis ligula vulputate posuere egestas. Etiam vestibulum, lectus sit amet rhoncus facilisis, nunc arcu hendrerit neque, ac auctor eros velit ut nisi.",
                            Name = "Benzema",
                            Email = "bel@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Aenean id lobortis nulla. Phasellus pretium elit nec ullamcorper bibendum. Nullam bibendum erat a mi rhoncus iaculis eu sit amet mauris. Curabitur quam velit, sollicitudin at ultrices ut, mollis id augue. Nam dapibus facilisis sem. Nam malesuada nunc at est sollicitudin, a luctus sapien eleifend. Praesent mi purus",
                            Name = "Andrew",
                            Email = "andy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Mauris vestibulum dolor non est rutrum, quis consectetur quam pharetra. Donec a elementum sem. Quisque at nibh condimentum, fringilla diam et, volutpat magna. Sed consectetur vitae mauris et rhoncus. Etiam eget odio dolor. Sed vel justo nec est pellentesque volutpat eget eget enim",
                            Name = "Lumi",
                            Email = "lucas@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Duis at blandit purus. Suspendisse potenti. Nam ac ornare ante. Aliquam posuere ultricies turpis et laoreet. Nam eleifend magna et nulla ultricies, sit amet fringilla ante varius. Donec blandit massa quam, nec finibus turpis dapibus id. Integer aliquet malesuada turpis eget euismod. ",
                            Name = "Jogn cena",
                            Email = "cena@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Pellentesque malesuada, est nec egestas vulputate, velit mauris vehicula velit,.",
                            Name = "Cynthia",
                            Email = "cynrthia@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Vestibulum tristique non magna gravida blandit. Duis consequat sodales massa ut sollicitudin. Donec imperdiet congue lectus eu mattis. Mauris pharetra blandit tincidunt. Aliquam erat mi, faucibus at tortor ut, elementum dictum leo. Proin nec volutpat dui.",
                            Name = "Lionel messi",
                            Email = "lmessi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Etiam dictum nec lorem in dignissim. Nam et ex volutpat, ultrices augue vel, aliquam tortor. Nunc volutpat euismod tortor.",
                            Name = "Ethanla",
                            Email = "thye@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Morbi hendrerit lectus in magna mattis, sed scelerisque leo mattis. Nam blandit commodo turpis ut ornare. Donec non nisl ornare, ullamcorper massa quis, ultricies ipsum. Donec at sapien ac libero laoreet finibus. Sed ullamcorper nulla eu venenatis placerat. In sed lobortis eros. Nunc ut venenatis tortor",
                            Name = "Gabriel Victor",
                            Email = "gabriel@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },



                    };
                _reviewService.AddRangeReveiew(reviews);

            }
            return View();
        }


        public ActionResult Review5()
        {
            var products = _productService.GetAllProducts().Skip(40).Take(10);

            Random rand = new Random();

            foreach (var product in products)
            {
                var reviews = new List<Review>()
                    {
                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.",
                            Name = "James Victor",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "using Content here, content here, making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for lorem ipsum will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                            Name="Victor Daniel",
                            Email = "balam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old.",
                            Name = "Adekunle Gold",
                            Email = "AGbaby@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source.",
                            Name = "Olorunfemi John",
                            Email = "john@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. ",
                            Name = "Simi oyekunke",
                            Email = "simi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. book.",
                            Name = "Eniola",
                            Email = "Eny@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = " It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.",
                            Name = "Cornor",
                            Email = "cnor@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system",
                            Name = "Paul",
                            Email = "pp@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure.",
                            Name = "Opeyemi",
                            Email = "opy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "On the other hand, we denounce with righteous indignation and dislike men who are so beguiled and demoralized by the charms of pleasure of the moment, so blinded by desire, that they cannot foresee the pain and trouble that are bound to ensue; and equal blame belongs to those who fail in their duty through weakness of will, which is the same as saying through shrinking from toil and pain. These cases are perfectly simple and easy to distinguish. ",
                            Name = "KingBach",
                            Email = "king@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Mauris eu purus sed ipsum egestas ullamcorper. Aenean nec sem pretium velit lacinia varius at vel leo. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Aenean convallis ut nunc sit amet fermentum. Etiam non vehicula neque, iaculis luctus sem. Aenean efficitur, nibh a pellentesque rutrum, dui lorem sagittis ante, ut malesuada magna lacus quis dolor. Vestibulum feugiat commodo luctus",
                            Name = "Tresh",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Praesent fringilla egestas lacus, sit amet consequat neque ultrices ultrices. Etiam sit amet sem dolor. Praesent rhoncus tincidunt ex eget mollis. Cras maximus, enim eu tempor tristique, quam augue volutpat dolor, a volutpat nibh ipsum at sapien. Nam turpis arcu, elementum sed laoreet quis, commodo vel felis. Sed sed pretium felis, sed iaculis urna. Suspendisse scelerisque lacinia purus eget porta. Cras eget tempor lorem",
                            Name = "Femi",
                            Email = "femi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Curabitur ullamcorper ante sed porttitor maximus. Proin pharetra vitae nunc nec tristique. Curabitur aliquam tristique diam a aliquam. Nulla tellus diam, commodo non condimentum at, hendrerit eu diam. Curabitur facilisis nunc sit amet leo cursus, at tristique purus blandit.",
                            Name = "Victor",
                            Email = "victo@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Praesent fringilla egestas lacus, sit amet consequat neque ultrices ultrices. Etiam sit amet sem dolor. Praesent rhoncus tincidunt ex eget mollis. Cras maximus, enim eu tempor tristique, quam augue volutpat dolor, a volutpat nibh ipsum at sapien. Nam turpis arcu, elementum sed laoreet quis, commodo vel felis. Sed sed pretium felis, sed iaculis urna. Suspendisse scelerisque lacinia purus eget porta. Cras eget tempor lorem.",
                            Name = "Grizmann",
                            Email = "grizzy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Mauris mollis auctor velit, vitae scelerisque mauris feugiat sed. Mauris eget cursus tortor. Curabitur a dui in turpis consectetur gravida",
                            Name = "Sandra",
                            Email = "sandy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Aenean vel egestas dolor. Phasellus ac dictum tellus. Nam vel ante sit amet velit finibus finibus quis ut nisi. Mauris et varius est. Mauris vel efficitur odio. Duis faucibus nulla lacus, vel feugiat dui consectetur et.",
                            Name = "Saul goodman",
                            Email = "saulG@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Duis aliquet, enim lobortis ultricies euismod, risus neque tincidunt risus, in vestibulum erat nunc vitae nisl. In congue erat vel ultricies egestas. Vestibulum placerat, elit non condimentum maximus, sapien ligula porttitor orci, at pellentesque est velit non est. Ut efficitur ligula magna, id laoreet est faucibus non.",
                            Name = "Kendel",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = " Donec fermentum lacus et lacus pharetra, ut molestie justo semper. Curabitur id ultricies tortor. Fusce quis elit sit amet velit vulputate molestie. Vivamus at euismod ex. Etiam sit amet porttitor ligula.",
                            Name = "Jet Victor",
                            Email = "jv@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Duis aliquet, enim lobortis ultricies euismod, risus neque tincidunt risus, in vestibulum erat nunc vitae nisl. In congue erat vel ultricies egestas. Vestibulum placerat, elit non condimentum maximus, sapien ligula porttitor orci, at pellentesque est velit non est. Ut efficitur ligula magna, id laoreet est faucibus non.",
                            Name = "Walter white",
                            Email = "walter@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Aliquam euismod nisl imperdiet enim consectetur, eu dapibus massa placerat. Phasellus cursus vehicula mi, at tincidunt odio consectetur at. Integer neque turpis, sagittis vel ullamcorper imperdiet, ullamcorper quis dolor. Donec vitae rhoncus nunc. Maecenas placerat arcu in nisi viverra viverra.",
                            Name = "Dembele",
                            Email = "dd@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Nam tempus fringilla ligula, eget consequat dolor tincidunt id. Aliquam tincidunt, lectus ac hendrerit cursus, dui odio facilisis ligula, et placerat nisi leo quis nunc. Donec fermentum lacus et lacus pharetra, ut molestie justo semper. Curabitur id ultricies tortor. Fusce quis elit sit amet velit vulputate molestie. Vivamus at euismod ex. Etiam sit amet porttitor ligula.",
                            Name = "Suarez",
                            Email = "suarez@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Donec at ante a ante commodo consequat. Donec at cursus justo. Phasellus pretium elit sit amet enim placerat, non volutpat ligula sollicitudin. Curabitur a sem ac enim lacinia fringilla in nec nisl. Suspendisse potenti. Maecenas orci quam, eleifend eu metus at, placerat imperdiet elit. ",
                            Name = "Donald",
                            Email = "don@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Curabitur pellentesque elementum mauris, ac vulputate purus vehicula at. Pellentesque non tortor ornare, consectetur nisl et, hendrerit quam. Morbi mattis accumsan purus eget ornare. Donec interdum tristique nulla, rutrum posuere felis tincidunt et. Praesent suscipit et nulla nec scelerisque.",
                            Name = "Shayme",
                            Email = "sh@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = " Fusce vehicula lectus in ex aliquam sollicitudin. Nunc nec laoreet tellus. Mauris eu elit a nulla ultrices lobortis in condimentum mi. Curabitur porta nunc quis faucibus venenatis.",
                            Name = "torbido",
                            Email = "tr@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Phasellus eleifend pharetra urna non maximus. Nulla in pretium risus. Sed aliquet tempor felis ac faucibus. Nulla posuere elit nec erat elementum egestas. Aenean convallis ligula vulputate posuere egestas. Etiam vestibulum, lectus sit amet rhoncus facilisis, nunc arcu hendrerit neque, ac auctor eros velit ut nisi.",
                            Name = "Benzema",
                            Email = "bel@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Aenean id lobortis nulla. Phasellus pretium elit nec ullamcorper bibendum. Nullam bibendum erat a mi rhoncus iaculis eu sit amet mauris. Curabitur quam velit, sollicitudin at ultrices ut, mollis id augue. Nam dapibus facilisis sem. Nam malesuada nunc at est sollicitudin, a luctus sapien eleifend. Praesent mi purus",
                            Name = "Andrew",
                            Email = "andy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Mauris vestibulum dolor non est rutrum, quis consectetur quam pharetra. Donec a elementum sem. Quisque at nibh condimentum, fringilla diam et, volutpat magna. Sed consectetur vitae mauris et rhoncus. Etiam eget odio dolor. Sed vel justo nec est pellentesque volutpat eget eget enim",
                            Name = "Lumi",
                            Email = "lucas@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Duis at blandit purus. Suspendisse potenti. Nam ac ornare ante. Aliquam posuere ultricies turpis et laoreet. Nam eleifend magna et nulla ultricies, sit amet fringilla ante varius. Donec blandit massa quam, nec finibus turpis dapibus id. Integer aliquet malesuada turpis eget euismod. ",
                            Name = "Jogn cena",
                            Email = "cena@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Pellentesque malesuada, est nec egestas vulputate, velit mauris vehicula velit,.",
                            Name = "Cynthia",
                            Email = "cynrthia@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Vestibulum tristique non magna gravida blandit. Duis consequat sodales massa ut sollicitudin. Donec imperdiet congue lectus eu mattis. Mauris pharetra blandit tincidunt. Aliquam erat mi, faucibus at tortor ut, elementum dictum leo. Proin nec volutpat dui.",
                            Name = "Lionel messi",
                            Email = "lmessi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Etiam dictum nec lorem in dignissim. Nam et ex volutpat, ultrices augue vel, aliquam tortor. Nunc volutpat euismod tortor.",
                            Name = "Ethanla",
                            Email = "thye@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                        DateCreatedUtc = DateTime.UtcNow,DateModifiedUtc = DateTime.UtcNow,
                            Comment = "Morbi hendrerit lectus in magna mattis, sed scelerisque leo mattis. Nam blandit commodo turpis ut ornare. Donec non nisl ornare, ullamcorper massa quis, ultricies ipsum. Donec at sapien ac libero laoreet finibus. Sed ullamcorper nulla eu venenatis placerat. In sed lobortis eros. Nunc ut venenatis tortor",
                            Name = "Gabriel Victor",
                            Email = "gabriel@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },



                    };
                _reviewService.AddRangeReveiew(reviews);

            }
            return View();
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
    }
}