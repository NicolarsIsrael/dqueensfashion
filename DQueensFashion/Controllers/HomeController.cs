using DQueensFashion.Core.Model;
using DQueensFashion.CustomFilters;
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
    [HomeSetGlobalVariable]
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ICustomerService _customerService;
        private readonly IWishListService _wishListService;
        private readonly ILineItemService _lineItemService;
        private readonly IOrderService _orderService;
        private readonly IReviewService _reviewService;
        private readonly IImageService _imageService;

        public HomeController(IProductService productService, ICategoryService categoryService,ICustomerService customerService, IWishListService wishListService,
            ILineItemService lineItemService, IOrderService orderService, IReviewService reviewService, IImageService imageService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _customerService = customerService;
            _wishListService = wishListService;
            _lineItemService = lineItemService;
            _orderService = orderService;
            _reviewService = reviewService;
            _imageService = imageService;
        }
        public ActionResult Index()
        {
            var allImages = _imageService.GetAllImageFiles().ToList();

            IEnumerable<ViewProductsViewModel> products = _productService.GetAllProducts()
                .Select(p => new ViewProductsViewModel()
                {
                    Id = p.Id,
                    Name = p.Name.Length > 17 ? p.Name.Substring(0, 15) + "..." : p.Name,
                    MainImage = allImages.Where(image=>image.ProductId==p.Id).Count()<1?
                        AppConstant.DefaultProductImage :
                        allImages.Where(image => image.ProductId == p.Id).FirstOrDefault().ImagePath,
                    Quantity = p.Quantity,
                    Price=p.Price.ToString(),
                    Discount = p.Discount,
                    SubTotal = p.SubTotal.ToString(),
                    Category = p.Category.Name,
                    CategoryId = p.Category.Id,
                    Rating = new RatingViewModel()
                    {
                        AverageRating = p.AverageRating.ToString("0.0"),
                        TotalReviewCount = _reviewService.GetReviewCountForProduct(p.Id).ToString(),
                        IsDouble = (p.AverageRating % 1) == 0 ? false : true,
                        FloorAverageRating = (int)Math.Floor(p.AverageRating)
                    },
                    
                }).ToList();

            IEnumerable<CategoryNameAndId> categories = _categoryService.GetAllCategories()
                .Select(category => new CategoryNameAndId()
                {
                    Id = category.Id,
                    Name = category.Name.ToUpper(),
                }).OrderBy(c=>c.Name).ToList();

            HomeIndexViewModel homeIndex = new HomeIndexViewModel()
            {
                Products = products,
                Categories = categories,
            };

            return View(homeIndex);
        }


        public ActionResult About()
        {

            //var products = _productService.GetAllProducts();
            //foreach(var product in products)
            //{
            //    product.AverageRating = _reviewService.GetAverageRating(product.Id);
            //    product.SubTotal = _productService.CalculateProductPrice(product.Price, product.Discount);
            //    _productService.UpdateProduct(product);
            //}

            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Review1()
        {
            var products = _productService.GetAllProducts().Take(10);

            Random rand = new Random();

            foreach (var product in products)
            {
                var reviews = new List<Review>()
                    {
                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.",
                            Name = "James Victor",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "using Content here, content here, making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for lorem ipsum will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                            Name="Victor Daniel",
                            Email = "balam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old.",
                            Name = "Adekunle Gold",
                            Email = "AGbaby@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source.",
                            Name = "Olorunfemi John",
                            Email = "john@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. ",
                            Name = "Simi oyekunke",
                            Email = "simi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. book.",
                            Name = "Eniola",
                            Email = "Eny@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = " It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.",
                            Name = "Cornor",
                            Email = "cnor@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system",
                            Name = "Paul",
                            Email = "pp@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure.",
                            Name = "Opeyemi",
                            Email = "opy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "On the other hand, we denounce with righteous indignation and dislike men who are so beguiled and demoralized by the charms of pleasure of the moment, so blinded by desire, that they cannot foresee the pain and trouble that are bound to ensue; and equal blame belongs to those who fail in their duty through weakness of will, which is the same as saying through shrinking from toil and pain. These cases are perfectly simple and easy to distinguish. ",
                            Name = "KingBach",
                            Email = "king@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Mauris eu purus sed ipsum egestas ullamcorper. Aenean nec sem pretium velit lacinia varius at vel leo. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Aenean convallis ut nunc sit amet fermentum. Etiam non vehicula neque, iaculis luctus sem. Aenean efficitur, nibh a pellentesque rutrum, dui lorem sagittis ante, ut malesuada magna lacus quis dolor. Vestibulum feugiat commodo luctus",
                            Name = "Tresh",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Praesent fringilla egestas lacus, sit amet consequat neque ultrices ultrices. Etiam sit amet sem dolor. Praesent rhoncus tincidunt ex eget mollis. Cras maximus, enim eu tempor tristique, quam augue volutpat dolor, a volutpat nibh ipsum at sapien. Nam turpis arcu, elementum sed laoreet quis, commodo vel felis. Sed sed pretium felis, sed iaculis urna. Suspendisse scelerisque lacinia purus eget porta. Cras eget tempor lorem",
                            Name = "Femi",
                            Email = "femi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Curabitur ullamcorper ante sed porttitor maximus. Proin pharetra vitae nunc nec tristique. Curabitur aliquam tristique diam a aliquam. Nulla tellus diam, commodo non condimentum at, hendrerit eu diam. Curabitur facilisis nunc sit amet leo cursus, at tristique purus blandit.",
                            Name = "Victor",
                            Email = "victo@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Praesent fringilla egestas lacus, sit amet consequat neque ultrices ultrices. Etiam sit amet sem dolor. Praesent rhoncus tincidunt ex eget mollis. Cras maximus, enim eu tempor tristique, quam augue volutpat dolor, a volutpat nibh ipsum at sapien. Nam turpis arcu, elementum sed laoreet quis, commodo vel felis. Sed sed pretium felis, sed iaculis urna. Suspendisse scelerisque lacinia purus eget porta. Cras eget tempor lorem.",
                            Name = "Grizmann",
                            Email = "grizzy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Mauris mollis auctor velit, vitae scelerisque mauris feugiat sed. Mauris eget cursus tortor. Curabitur a dui in turpis consectetur gravida",
                            Name = "Sandra",
                            Email = "sandy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Aenean vel egestas dolor. Phasellus ac dictum tellus. Nam vel ante sit amet velit finibus finibus quis ut nisi. Mauris et varius est. Mauris vel efficitur odio. Duis faucibus nulla lacus, vel feugiat dui consectetur et.",
                            Name = "Saul goodman",
                            Email = "saulG@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Duis aliquet, enim lobortis ultricies euismod, risus neque tincidunt risus, in vestibulum erat nunc vitae nisl. In congue erat vel ultricies egestas. Vestibulum placerat, elit non condimentum maximus, sapien ligula porttitor orci, at pellentesque est velit non est. Ut efficitur ligula magna, id laoreet est faucibus non.",
                            Name = "Kendel",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = " Donec fermentum lacus et lacus pharetra, ut molestie justo semper. Curabitur id ultricies tortor. Fusce quis elit sit amet velit vulputate molestie. Vivamus at euismod ex. Etiam sit amet porttitor ligula.",
                            Name = "Jet Victor",
                            Email = "jv@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Duis aliquet, enim lobortis ultricies euismod, risus neque tincidunt risus, in vestibulum erat nunc vitae nisl. In congue erat vel ultricies egestas. Vestibulum placerat, elit non condimentum maximus, sapien ligula porttitor orci, at pellentesque est velit non est. Ut efficitur ligula magna, id laoreet est faucibus non.",
                            Name = "Walter white",
                            Email = "walter@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Aliquam euismod nisl imperdiet enim consectetur, eu dapibus massa placerat. Phasellus cursus vehicula mi, at tincidunt odio consectetur at. Integer neque turpis, sagittis vel ullamcorper imperdiet, ullamcorper quis dolor. Donec vitae rhoncus nunc. Maecenas placerat arcu in nisi viverra viverra.",
                            Name = "Dembele",
                            Email = "dd@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Nam tempus fringilla ligula, eget consequat dolor tincidunt id. Aliquam tincidunt, lectus ac hendrerit cursus, dui odio facilisis ligula, et placerat nisi leo quis nunc. Donec fermentum lacus et lacus pharetra, ut molestie justo semper. Curabitur id ultricies tortor. Fusce quis elit sit amet velit vulputate molestie. Vivamus at euismod ex. Etiam sit amet porttitor ligula.",
                            Name = "Suarez",
                            Email = "suarez@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Donec at ante a ante commodo consequat. Donec at cursus justo. Phasellus pretium elit sit amet enim placerat, non volutpat ligula sollicitudin. Curabitur a sem ac enim lacinia fringilla in nec nisl. Suspendisse potenti. Maecenas orci quam, eleifend eu metus at, placerat imperdiet elit. ",
                            Name = "Donald",
                            Email = "don@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Curabitur pellentesque elementum mauris, ac vulputate purus vehicula at. Pellentesque non tortor ornare, consectetur nisl et, hendrerit quam. Morbi mattis accumsan purus eget ornare. Donec interdum tristique nulla, rutrum posuere felis tincidunt et. Praesent suscipit et nulla nec scelerisque.",
                            Name = "Shayme",
                            Email = "sh@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = " Fusce vehicula lectus in ex aliquam sollicitudin. Nunc nec laoreet tellus. Mauris eu elit a nulla ultrices lobortis in condimentum mi. Curabitur porta nunc quis faucibus venenatis.",
                            Name = "torbido",
                            Email = "tr@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Phasellus eleifend pharetra urna non maximus. Nulla in pretium risus. Sed aliquet tempor felis ac faucibus. Nulla posuere elit nec erat elementum egestas. Aenean convallis ligula vulputate posuere egestas. Etiam vestibulum, lectus sit amet rhoncus facilisis, nunc arcu hendrerit neque, ac auctor eros velit ut nisi.",
                            Name = "Benzema",
                            Email = "bel@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Aenean id lobortis nulla. Phasellus pretium elit nec ullamcorper bibendum. Nullam bibendum erat a mi rhoncus iaculis eu sit amet mauris. Curabitur quam velit, sollicitudin at ultrices ut, mollis id augue. Nam dapibus facilisis sem. Nam malesuada nunc at est sollicitudin, a luctus sapien eleifend. Praesent mi purus",
                            Name = "Andrew",
                            Email = "andy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Mauris vestibulum dolor non est rutrum, quis consectetur quam pharetra. Donec a elementum sem. Quisque at nibh condimentum, fringilla diam et, volutpat magna. Sed consectetur vitae mauris et rhoncus. Etiam eget odio dolor. Sed vel justo nec est pellentesque volutpat eget eget enim",
                            Name = "Lumi",
                            Email = "lucas@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Duis at blandit purus. Suspendisse potenti. Nam ac ornare ante. Aliquam posuere ultricies turpis et laoreet. Nam eleifend magna et nulla ultricies, sit amet fringilla ante varius. Donec blandit massa quam, nec finibus turpis dapibus id. Integer aliquet malesuada turpis eget euismod. ",
                            Name = "Jogn cena",
                            Email = "cena@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Pellentesque malesuada, est nec egestas vulputate, velit mauris vehicula velit,.",
                            Name = "Cynthia",
                            Email = "cynrthia@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Vestibulum tristique non magna gravida blandit. Duis consequat sodales massa ut sollicitudin. Donec imperdiet congue lectus eu mattis. Mauris pharetra blandit tincidunt. Aliquam erat mi, faucibus at tortor ut, elementum dictum leo. Proin nec volutpat dui.",
                            Name = "Lionel messi",
                            Email = "lmessi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Etiam dictum nec lorem in dignissim. Nam et ex volutpat, ultrices augue vel, aliquam tortor. Nunc volutpat euismod tortor.",
                            Name = "Ethanla",
                            Email = "thye@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
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

        public ActionResult Review2()
        {
            var products = _productService.GetAllProducts().Skip(10).Take(10);

            Random rand = new Random();

            foreach (var product in products)
            {
                var reviews = new List<Review>()
                    {
                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.",
                            Name = "James Victor",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "using Content here, content here, making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for lorem ipsum will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                            Name="Victor Daniel",
                            Email = "balam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old.",
                            Name = "Adekunle Gold",
                            Email = "AGbaby@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source.",
                            Name = "Olorunfemi John",
                            Email = "john@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. ",
                            Name = "Simi oyekunke",
                            Email = "simi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. book.",
                            Name = "Eniola",
                            Email = "Eny@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = " It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.",
                            Name = "Cornor",
                            Email = "cnor@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system",
                            Name = "Paul",
                            Email = "pp@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure.",
                            Name = "Opeyemi",
                            Email = "opy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "On the other hand, we denounce with righteous indignation and dislike men who are so beguiled and demoralized by the charms of pleasure of the moment, so blinded by desire, that they cannot foresee the pain and trouble that are bound to ensue; and equal blame belongs to those who fail in their duty through weakness of will, which is the same as saying through shrinking from toil and pain. These cases are perfectly simple and easy to distinguish. ",
                            Name = "KingBach",
                            Email = "king@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Mauris eu purus sed ipsum egestas ullamcorper. Aenean nec sem pretium velit lacinia varius at vel leo. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Aenean convallis ut nunc sit amet fermentum. Etiam non vehicula neque, iaculis luctus sem. Aenean efficitur, nibh a pellentesque rutrum, dui lorem sagittis ante, ut malesuada magna lacus quis dolor. Vestibulum feugiat commodo luctus",
                            Name = "Tresh",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Praesent fringilla egestas lacus, sit amet consequat neque ultrices ultrices. Etiam sit amet sem dolor. Praesent rhoncus tincidunt ex eget mollis. Cras maximus, enim eu tempor tristique, quam augue volutpat dolor, a volutpat nibh ipsum at sapien. Nam turpis arcu, elementum sed laoreet quis, commodo vel felis. Sed sed pretium felis, sed iaculis urna. Suspendisse scelerisque lacinia purus eget porta. Cras eget tempor lorem",
                            Name = "Femi",
                            Email = "femi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Curabitur ullamcorper ante sed porttitor maximus. Proin pharetra vitae nunc nec tristique. Curabitur aliquam tristique diam a aliquam. Nulla tellus diam, commodo non condimentum at, hendrerit eu diam. Curabitur facilisis nunc sit amet leo cursus, at tristique purus blandit.",
                            Name = "Victor",
                            Email = "victo@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Praesent fringilla egestas lacus, sit amet consequat neque ultrices ultrices. Etiam sit amet sem dolor. Praesent rhoncus tincidunt ex eget mollis. Cras maximus, enim eu tempor tristique, quam augue volutpat dolor, a volutpat nibh ipsum at sapien. Nam turpis arcu, elementum sed laoreet quis, commodo vel felis. Sed sed pretium felis, sed iaculis urna. Suspendisse scelerisque lacinia purus eget porta. Cras eget tempor lorem.",
                            Name = "Grizmann",
                            Email = "grizzy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Mauris mollis auctor velit, vitae scelerisque mauris feugiat sed. Mauris eget cursus tortor. Curabitur a dui in turpis consectetur gravida",
                            Name = "Sandra",
                            Email = "sandy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Aenean vel egestas dolor. Phasellus ac dictum tellus. Nam vel ante sit amet velit finibus finibus quis ut nisi. Mauris et varius est. Mauris vel efficitur odio. Duis faucibus nulla lacus, vel feugiat dui consectetur et.",
                            Name = "Saul goodman",
                            Email = "saulG@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Duis aliquet, enim lobortis ultricies euismod, risus neque tincidunt risus, in vestibulum erat nunc vitae nisl. In congue erat vel ultricies egestas. Vestibulum placerat, elit non condimentum maximus, sapien ligula porttitor orci, at pellentesque est velit non est. Ut efficitur ligula magna, id laoreet est faucibus non.",
                            Name = "Kendel",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = " Donec fermentum lacus et lacus pharetra, ut molestie justo semper. Curabitur id ultricies tortor. Fusce quis elit sit amet velit vulputate molestie. Vivamus at euismod ex. Etiam sit amet porttitor ligula.",
                            Name = "Jet Victor",
                            Email = "jv@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Duis aliquet, enim lobortis ultricies euismod, risus neque tincidunt risus, in vestibulum erat nunc vitae nisl. In congue erat vel ultricies egestas. Vestibulum placerat, elit non condimentum maximus, sapien ligula porttitor orci, at pellentesque est velit non est. Ut efficitur ligula magna, id laoreet est faucibus non.",
                            Name = "Walter white",
                            Email = "walter@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Aliquam euismod nisl imperdiet enim consectetur, eu dapibus massa placerat. Phasellus cursus vehicula mi, at tincidunt odio consectetur at. Integer neque turpis, sagittis vel ullamcorper imperdiet, ullamcorper quis dolor. Donec vitae rhoncus nunc. Maecenas placerat arcu in nisi viverra viverra.",
                            Name = "Dembele",
                            Email = "dd@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Nam tempus fringilla ligula, eget consequat dolor tincidunt id. Aliquam tincidunt, lectus ac hendrerit cursus, dui odio facilisis ligula, et placerat nisi leo quis nunc. Donec fermentum lacus et lacus pharetra, ut molestie justo semper. Curabitur id ultricies tortor. Fusce quis elit sit amet velit vulputate molestie. Vivamus at euismod ex. Etiam sit amet porttitor ligula.",
                            Name = "Suarez",
                            Email = "suarez@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Donec at ante a ante commodo consequat. Donec at cursus justo. Phasellus pretium elit sit amet enim placerat, non volutpat ligula sollicitudin. Curabitur a sem ac enim lacinia fringilla in nec nisl. Suspendisse potenti. Maecenas orci quam, eleifend eu metus at, placerat imperdiet elit. ",
                            Name = "Donald",
                            Email = "don@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Curabitur pellentesque elementum mauris, ac vulputate purus vehicula at. Pellentesque non tortor ornare, consectetur nisl et, hendrerit quam. Morbi mattis accumsan purus eget ornare. Donec interdum tristique nulla, rutrum posuere felis tincidunt et. Praesent suscipit et nulla nec scelerisque.",
                            Name = "Shayme",
                            Email = "sh@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = " Fusce vehicula lectus in ex aliquam sollicitudin. Nunc nec laoreet tellus. Mauris eu elit a nulla ultrices lobortis in condimentum mi. Curabitur porta nunc quis faucibus venenatis.",
                            Name = "torbido",
                            Email = "tr@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Phasellus eleifend pharetra urna non maximus. Nulla in pretium risus. Sed aliquet tempor felis ac faucibus. Nulla posuere elit nec erat elementum egestas. Aenean convallis ligula vulputate posuere egestas. Etiam vestibulum, lectus sit amet rhoncus facilisis, nunc arcu hendrerit neque, ac auctor eros velit ut nisi.",
                            Name = "Benzema",
                            Email = "bel@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Aenean id lobortis nulla. Phasellus pretium elit nec ullamcorper bibendum. Nullam bibendum erat a mi rhoncus iaculis eu sit amet mauris. Curabitur quam velit, sollicitudin at ultrices ut, mollis id augue. Nam dapibus facilisis sem. Nam malesuada nunc at est sollicitudin, a luctus sapien eleifend. Praesent mi purus",
                            Name = "Andrew",
                            Email = "andy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Mauris vestibulum dolor non est rutrum, quis consectetur quam pharetra. Donec a elementum sem. Quisque at nibh condimentum, fringilla diam et, volutpat magna. Sed consectetur vitae mauris et rhoncus. Etiam eget odio dolor. Sed vel justo nec est pellentesque volutpat eget eget enim",
                            Name = "Lumi",
                            Email = "lucas@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Duis at blandit purus. Suspendisse potenti. Nam ac ornare ante. Aliquam posuere ultricies turpis et laoreet. Nam eleifend magna et nulla ultricies, sit amet fringilla ante varius. Donec blandit massa quam, nec finibus turpis dapibus id. Integer aliquet malesuada turpis eget euismod. ",
                            Name = "Jogn cena",
                            Email = "cena@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Pellentesque malesuada, est nec egestas vulputate, velit mauris vehicula velit,.",
                            Name = "Cynthia",
                            Email = "cynrthia@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Vestibulum tristique non magna gravida blandit. Duis consequat sodales massa ut sollicitudin. Donec imperdiet congue lectus eu mattis. Mauris pharetra blandit tincidunt. Aliquam erat mi, faucibus at tortor ut, elementum dictum leo. Proin nec volutpat dui.",
                            Name = "Lionel messi",
                            Email = "lmessi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Etiam dictum nec lorem in dignissim. Nam et ex volutpat, ultrices augue vel, aliquam tortor. Nunc volutpat euismod tortor.",
                            Name = "Ethanla",
                            Email = "thye@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
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
                            Comment = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.",
                            Name = "James Victor",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "using Content here, content here, making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for lorem ipsum will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                            Name="Victor Daniel",
                            Email = "balam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old.",
                            Name = "Adekunle Gold",
                            Email = "AGbaby@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source.",
                            Name = "Olorunfemi John",
                            Email = "john@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. ",
                            Name = "Simi oyekunke",
                            Email = "simi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. book.",
                            Name = "Eniola",
                            Email = "Eny@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = " It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.",
                            Name = "Cornor",
                            Email = "cnor@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system",
                            Name = "Paul",
                            Email = "pp@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure.",
                            Name = "Opeyemi",
                            Email = "opy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "On the other hand, we denounce with righteous indignation and dislike men who are so beguiled and demoralized by the charms of pleasure of the moment, so blinded by desire, that they cannot foresee the pain and trouble that are bound to ensue; and equal blame belongs to those who fail in their duty through weakness of will, which is the same as saying through shrinking from toil and pain. These cases are perfectly simple and easy to distinguish. ",
                            Name = "KingBach",
                            Email = "king@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Mauris eu purus sed ipsum egestas ullamcorper. Aenean nec sem pretium velit lacinia varius at vel leo. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Aenean convallis ut nunc sit amet fermentum. Etiam non vehicula neque, iaculis luctus sem. Aenean efficitur, nibh a pellentesque rutrum, dui lorem sagittis ante, ut malesuada magna lacus quis dolor. Vestibulum feugiat commodo luctus",
                            Name = "Tresh",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Praesent fringilla egestas lacus, sit amet consequat neque ultrices ultrices. Etiam sit amet sem dolor. Praesent rhoncus tincidunt ex eget mollis. Cras maximus, enim eu tempor tristique, quam augue volutpat dolor, a volutpat nibh ipsum at sapien. Nam turpis arcu, elementum sed laoreet quis, commodo vel felis. Sed sed pretium felis, sed iaculis urna. Suspendisse scelerisque lacinia purus eget porta. Cras eget tempor lorem",
                            Name = "Femi",
                            Email = "femi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Curabitur ullamcorper ante sed porttitor maximus. Proin pharetra vitae nunc nec tristique. Curabitur aliquam tristique diam a aliquam. Nulla tellus diam, commodo non condimentum at, hendrerit eu diam. Curabitur facilisis nunc sit amet leo cursus, at tristique purus blandit.",
                            Name = "Victor",
                            Email = "victo@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Praesent fringilla egestas lacus, sit amet consequat neque ultrices ultrices. Etiam sit amet sem dolor. Praesent rhoncus tincidunt ex eget mollis. Cras maximus, enim eu tempor tristique, quam augue volutpat dolor, a volutpat nibh ipsum at sapien. Nam turpis arcu, elementum sed laoreet quis, commodo vel felis. Sed sed pretium felis, sed iaculis urna. Suspendisse scelerisque lacinia purus eget porta. Cras eget tempor lorem.",
                            Name = "Grizmann",
                            Email = "grizzy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Mauris mollis auctor velit, vitae scelerisque mauris feugiat sed. Mauris eget cursus tortor. Curabitur a dui in turpis consectetur gravida",
                            Name = "Sandra",
                            Email = "sandy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Aenean vel egestas dolor. Phasellus ac dictum tellus. Nam vel ante sit amet velit finibus finibus quis ut nisi. Mauris et varius est. Mauris vel efficitur odio. Duis faucibus nulla lacus, vel feugiat dui consectetur et.",
                            Name = "Saul goodman",
                            Email = "saulG@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Duis aliquet, enim lobortis ultricies euismod, risus neque tincidunt risus, in vestibulum erat nunc vitae nisl. In congue erat vel ultricies egestas. Vestibulum placerat, elit non condimentum maximus, sapien ligula porttitor orci, at pellentesque est velit non est. Ut efficitur ligula magna, id laoreet est faucibus non.",
                            Name = "Kendel",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = " Donec fermentum lacus et lacus pharetra, ut molestie justo semper. Curabitur id ultricies tortor. Fusce quis elit sit amet velit vulputate molestie. Vivamus at euismod ex. Etiam sit amet porttitor ligula.",
                            Name = "Jet Victor",
                            Email = "jv@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Duis aliquet, enim lobortis ultricies euismod, risus neque tincidunt risus, in vestibulum erat nunc vitae nisl. In congue erat vel ultricies egestas. Vestibulum placerat, elit non condimentum maximus, sapien ligula porttitor orci, at pellentesque est velit non est. Ut efficitur ligula magna, id laoreet est faucibus non.",
                            Name = "Walter white",
                            Email = "walter@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Aliquam euismod nisl imperdiet enim consectetur, eu dapibus massa placerat. Phasellus cursus vehicula mi, at tincidunt odio consectetur at. Integer neque turpis, sagittis vel ullamcorper imperdiet, ullamcorper quis dolor. Donec vitae rhoncus nunc. Maecenas placerat arcu in nisi viverra viverra.",
                            Name = "Dembele",
                            Email = "dd@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Nam tempus fringilla ligula, eget consequat dolor tincidunt id. Aliquam tincidunt, lectus ac hendrerit cursus, dui odio facilisis ligula, et placerat nisi leo quis nunc. Donec fermentum lacus et lacus pharetra, ut molestie justo semper. Curabitur id ultricies tortor. Fusce quis elit sit amet velit vulputate molestie. Vivamus at euismod ex. Etiam sit amet porttitor ligula.",
                            Name = "Suarez",
                            Email = "suarez@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Donec at ante a ante commodo consequat. Donec at cursus justo. Phasellus pretium elit sit amet enim placerat, non volutpat ligula sollicitudin. Curabitur a sem ac enim lacinia fringilla in nec nisl. Suspendisse potenti. Maecenas orci quam, eleifend eu metus at, placerat imperdiet elit. ",
                            Name = "Donald",
                            Email = "don@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Curabitur pellentesque elementum mauris, ac vulputate purus vehicula at. Pellentesque non tortor ornare, consectetur nisl et, hendrerit quam. Morbi mattis accumsan purus eget ornare. Donec interdum tristique nulla, rutrum posuere felis tincidunt et. Praesent suscipit et nulla nec scelerisque.",
                            Name = "Shayme",
                            Email = "sh@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = " Fusce vehicula lectus in ex aliquam sollicitudin. Nunc nec laoreet tellus. Mauris eu elit a nulla ultrices lobortis in condimentum mi. Curabitur porta nunc quis faucibus venenatis.",
                            Name = "torbido",
                            Email = "tr@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Phasellus eleifend pharetra urna non maximus. Nulla in pretium risus. Sed aliquet tempor felis ac faucibus. Nulla posuere elit nec erat elementum egestas. Aenean convallis ligula vulputate posuere egestas. Etiam vestibulum, lectus sit amet rhoncus facilisis, nunc arcu hendrerit neque, ac auctor eros velit ut nisi.",
                            Name = "Benzema",
                            Email = "bel@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Aenean id lobortis nulla. Phasellus pretium elit nec ullamcorper bibendum. Nullam bibendum erat a mi rhoncus iaculis eu sit amet mauris. Curabitur quam velit, sollicitudin at ultrices ut, mollis id augue. Nam dapibus facilisis sem. Nam malesuada nunc at est sollicitudin, a luctus sapien eleifend. Praesent mi purus",
                            Name = "Andrew",
                            Email = "andy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Mauris vestibulum dolor non est rutrum, quis consectetur quam pharetra. Donec a elementum sem. Quisque at nibh condimentum, fringilla diam et, volutpat magna. Sed consectetur vitae mauris et rhoncus. Etiam eget odio dolor. Sed vel justo nec est pellentesque volutpat eget eget enim",
                            Name = "Lumi",
                            Email = "lucas@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Duis at blandit purus. Suspendisse potenti. Nam ac ornare ante. Aliquam posuere ultricies turpis et laoreet. Nam eleifend magna et nulla ultricies, sit amet fringilla ante varius. Donec blandit massa quam, nec finibus turpis dapibus id. Integer aliquet malesuada turpis eget euismod. ",
                            Name = "Jogn cena",
                            Email = "cena@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Pellentesque malesuada, est nec egestas vulputate, velit mauris vehicula velit,.",
                            Name = "Cynthia",
                            Email = "cynrthia@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Vestibulum tristique non magna gravida blandit. Duis consequat sodales massa ut sollicitudin. Donec imperdiet congue lectus eu mattis. Mauris pharetra blandit tincidunt. Aliquam erat mi, faucibus at tortor ut, elementum dictum leo. Proin nec volutpat dui.",
                            Name = "Lionel messi",
                            Email = "lmessi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Etiam dictum nec lorem in dignissim. Nam et ex volutpat, ultrices augue vel, aliquam tortor. Nunc volutpat euismod tortor.",
                            Name = "Ethanla",
                            Email = "thye@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
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
                            Comment = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.",
                            Name = "James Victor",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "using Content here, content here, making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for lorem ipsum will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                            Name="Victor Daniel",
                            Email = "balam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old.",
                            Name = "Adekunle Gold",
                            Email = "AGbaby@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source.",
                            Name = "Olorunfemi John",
                            Email = "john@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. ",
                            Name = "Simi oyekunke",
                            Email = "simi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. book.",
                            Name = "Eniola",
                            Email = "Eny@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = " It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.",
                            Name = "Cornor",
                            Email = "cnor@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system",
                            Name = "Paul",
                            Email = "pp@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure.",
                            Name = "Opeyemi",
                            Email = "opy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "On the other hand, we denounce with righteous indignation and dislike men who are so beguiled and demoralized by the charms of pleasure of the moment, so blinded by desire, that they cannot foresee the pain and trouble that are bound to ensue; and equal blame belongs to those who fail in their duty through weakness of will, which is the same as saying through shrinking from toil and pain. These cases are perfectly simple and easy to distinguish. ",
                            Name = "KingBach",
                            Email = "king@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Mauris eu purus sed ipsum egestas ullamcorper. Aenean nec sem pretium velit lacinia varius at vel leo. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Aenean convallis ut nunc sit amet fermentum. Etiam non vehicula neque, iaculis luctus sem. Aenean efficitur, nibh a pellentesque rutrum, dui lorem sagittis ante, ut malesuada magna lacus quis dolor. Vestibulum feugiat commodo luctus",
                            Name = "Tresh",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Praesent fringilla egestas lacus, sit amet consequat neque ultrices ultrices. Etiam sit amet sem dolor. Praesent rhoncus tincidunt ex eget mollis. Cras maximus, enim eu tempor tristique, quam augue volutpat dolor, a volutpat nibh ipsum at sapien. Nam turpis arcu, elementum sed laoreet quis, commodo vel felis. Sed sed pretium felis, sed iaculis urna. Suspendisse scelerisque lacinia purus eget porta. Cras eget tempor lorem",
                            Name = "Femi",
                            Email = "femi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Curabitur ullamcorper ante sed porttitor maximus. Proin pharetra vitae nunc nec tristique. Curabitur aliquam tristique diam a aliquam. Nulla tellus diam, commodo non condimentum at, hendrerit eu diam. Curabitur facilisis nunc sit amet leo cursus, at tristique purus blandit.",
                            Name = "Victor",
                            Email = "victo@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Praesent fringilla egestas lacus, sit amet consequat neque ultrices ultrices. Etiam sit amet sem dolor. Praesent rhoncus tincidunt ex eget mollis. Cras maximus, enim eu tempor tristique, quam augue volutpat dolor, a volutpat nibh ipsum at sapien. Nam turpis arcu, elementum sed laoreet quis, commodo vel felis. Sed sed pretium felis, sed iaculis urna. Suspendisse scelerisque lacinia purus eget porta. Cras eget tempor lorem.",
                            Name = "Grizmann",
                            Email = "grizzy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Mauris mollis auctor velit, vitae scelerisque mauris feugiat sed. Mauris eget cursus tortor. Curabitur a dui in turpis consectetur gravida",
                            Name = "Sandra",
                            Email = "sandy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Aenean vel egestas dolor. Phasellus ac dictum tellus. Nam vel ante sit amet velit finibus finibus quis ut nisi. Mauris et varius est. Mauris vel efficitur odio. Duis faucibus nulla lacus, vel feugiat dui consectetur et.",
                            Name = "Saul goodman",
                            Email = "saulG@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Duis aliquet, enim lobortis ultricies euismod, risus neque tincidunt risus, in vestibulum erat nunc vitae nisl. In congue erat vel ultricies egestas. Vestibulum placerat, elit non condimentum maximus, sapien ligula porttitor orci, at pellentesque est velit non est. Ut efficitur ligula magna, id laoreet est faucibus non.",
                            Name = "Kendel",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = " Donec fermentum lacus et lacus pharetra, ut molestie justo semper. Curabitur id ultricies tortor. Fusce quis elit sit amet velit vulputate molestie. Vivamus at euismod ex. Etiam sit amet porttitor ligula.",
                            Name = "Jet Victor",
                            Email = "jv@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Duis aliquet, enim lobortis ultricies euismod, risus neque tincidunt risus, in vestibulum erat nunc vitae nisl. In congue erat vel ultricies egestas. Vestibulum placerat, elit non condimentum maximus, sapien ligula porttitor orci, at pellentesque est velit non est. Ut efficitur ligula magna, id laoreet est faucibus non.",
                            Name = "Walter white",
                            Email = "walter@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Aliquam euismod nisl imperdiet enim consectetur, eu dapibus massa placerat. Phasellus cursus vehicula mi, at tincidunt odio consectetur at. Integer neque turpis, sagittis vel ullamcorper imperdiet, ullamcorper quis dolor. Donec vitae rhoncus nunc. Maecenas placerat arcu in nisi viverra viverra.",
                            Name = "Dembele",
                            Email = "dd@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Nam tempus fringilla ligula, eget consequat dolor tincidunt id. Aliquam tincidunt, lectus ac hendrerit cursus, dui odio facilisis ligula, et placerat nisi leo quis nunc. Donec fermentum lacus et lacus pharetra, ut molestie justo semper. Curabitur id ultricies tortor. Fusce quis elit sit amet velit vulputate molestie. Vivamus at euismod ex. Etiam sit amet porttitor ligula.",
                            Name = "Suarez",
                            Email = "suarez@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Donec at ante a ante commodo consequat. Donec at cursus justo. Phasellus pretium elit sit amet enim placerat, non volutpat ligula sollicitudin. Curabitur a sem ac enim lacinia fringilla in nec nisl. Suspendisse potenti. Maecenas orci quam, eleifend eu metus at, placerat imperdiet elit. ",
                            Name = "Donald",
                            Email = "don@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Curabitur pellentesque elementum mauris, ac vulputate purus vehicula at. Pellentesque non tortor ornare, consectetur nisl et, hendrerit quam. Morbi mattis accumsan purus eget ornare. Donec interdum tristique nulla, rutrum posuere felis tincidunt et. Praesent suscipit et nulla nec scelerisque.",
                            Name = "Shayme",
                            Email = "sh@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = " Fusce vehicula lectus in ex aliquam sollicitudin. Nunc nec laoreet tellus. Mauris eu elit a nulla ultrices lobortis in condimentum mi. Curabitur porta nunc quis faucibus venenatis.",
                            Name = "torbido",
                            Email = "tr@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Phasellus eleifend pharetra urna non maximus. Nulla in pretium risus. Sed aliquet tempor felis ac faucibus. Nulla posuere elit nec erat elementum egestas. Aenean convallis ligula vulputate posuere egestas. Etiam vestibulum, lectus sit amet rhoncus facilisis, nunc arcu hendrerit neque, ac auctor eros velit ut nisi.",
                            Name = "Benzema",
                            Email = "bel@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Aenean id lobortis nulla. Phasellus pretium elit nec ullamcorper bibendum. Nullam bibendum erat a mi rhoncus iaculis eu sit amet mauris. Curabitur quam velit, sollicitudin at ultrices ut, mollis id augue. Nam dapibus facilisis sem. Nam malesuada nunc at est sollicitudin, a luctus sapien eleifend. Praesent mi purus",
                            Name = "Andrew",
                            Email = "andy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Mauris vestibulum dolor non est rutrum, quis consectetur quam pharetra. Donec a elementum sem. Quisque at nibh condimentum, fringilla diam et, volutpat magna. Sed consectetur vitae mauris et rhoncus. Etiam eget odio dolor. Sed vel justo nec est pellentesque volutpat eget eget enim",
                            Name = "Lumi",
                            Email = "lucas@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Duis at blandit purus. Suspendisse potenti. Nam ac ornare ante. Aliquam posuere ultricies turpis et laoreet. Nam eleifend magna et nulla ultricies, sit amet fringilla ante varius. Donec blandit massa quam, nec finibus turpis dapibus id. Integer aliquet malesuada turpis eget euismod. ",
                            Name = "Jogn cena",
                            Email = "cena@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Pellentesque malesuada, est nec egestas vulputate, velit mauris vehicula velit,.",
                            Name = "Cynthia",
                            Email = "cynrthia@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Vestibulum tristique non magna gravida blandit. Duis consequat sodales massa ut sollicitudin. Donec imperdiet congue lectus eu mattis. Mauris pharetra blandit tincidunt. Aliquam erat mi, faucibus at tortor ut, elementum dictum leo. Proin nec volutpat dui.",
                            Name = "Lionel messi",
                            Email = "lmessi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Etiam dictum nec lorem in dignissim. Nam et ex volutpat, ultrices augue vel, aliquam tortor. Nunc volutpat euismod tortor.",
                            Name = "Ethanla",
                            Email = "thye@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
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
                            Comment = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.",
                            Name = "James Victor",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "using Content here, content here, making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for lorem ipsum will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                            Name="Victor Daniel",
                            Email = "balam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old.",
                            Name = "Adekunle Gold",
                            Email = "AGbaby@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source.",
                            Name = "Olorunfemi John",
                            Email = "john@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable. ",
                            Name = "Simi oyekunke",
                            Email = "simi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "If you are going to use a passage of Lorem Ipsum, you need to be sure there isn't anything embarrassing hidden in the middle of text. All the Lorem Ipsum generators on the Internet tend to repeat predefined chunks as necessary, making this the first true generator on the Internet. book.",
                            Name = "Eniola",
                            Email = "Eny@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = " It uses a dictionary of over 200 Latin words, combined with a handful of model sentence structures, to generate Lorem Ipsum which looks reasonable. The generated Lorem Ipsum is therefore always free from repetition, injected humour, or non-characteristic words etc.",
                            Name = "Cornor",
                            Email = "cnor@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system",
                            Name = "Paul",
                            Email = "pp@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur in which toil and pain can procure him some great pleasure.",
                            Name = "Opeyemi",
                            Email = "opy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "On the other hand, we denounce with righteous indignation and dislike men who are so beguiled and demoralized by the charms of pleasure of the moment, so blinded by desire, that they cannot foresee the pain and trouble that are bound to ensue; and equal blame belongs to those who fail in their duty through weakness of will, which is the same as saying through shrinking from toil and pain. These cases are perfectly simple and easy to distinguish. ",
                            Name = "KingBach",
                            Email = "king@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Mauris eu purus sed ipsum egestas ullamcorper. Aenean nec sem pretium velit lacinia varius at vel leo. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Aenean convallis ut nunc sit amet fermentum. Etiam non vehicula neque, iaculis luctus sem. Aenean efficitur, nibh a pellentesque rutrum, dui lorem sagittis ante, ut malesuada magna lacus quis dolor. Vestibulum feugiat commodo luctus",
                            Name = "Tresh",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Praesent fringilla egestas lacus, sit amet consequat neque ultrices ultrices. Etiam sit amet sem dolor. Praesent rhoncus tincidunt ex eget mollis. Cras maximus, enim eu tempor tristique, quam augue volutpat dolor, a volutpat nibh ipsum at sapien. Nam turpis arcu, elementum sed laoreet quis, commodo vel felis. Sed sed pretium felis, sed iaculis urna. Suspendisse scelerisque lacinia purus eget porta. Cras eget tempor lorem",
                            Name = "Femi",
                            Email = "femi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Curabitur ullamcorper ante sed porttitor maximus. Proin pharetra vitae nunc nec tristique. Curabitur aliquam tristique diam a aliquam. Nulla tellus diam, commodo non condimentum at, hendrerit eu diam. Curabitur facilisis nunc sit amet leo cursus, at tristique purus blandit.",
                            Name = "Victor",
                            Email = "victo@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Praesent fringilla egestas lacus, sit amet consequat neque ultrices ultrices. Etiam sit amet sem dolor. Praesent rhoncus tincidunt ex eget mollis. Cras maximus, enim eu tempor tristique, quam augue volutpat dolor, a volutpat nibh ipsum at sapien. Nam turpis arcu, elementum sed laoreet quis, commodo vel felis. Sed sed pretium felis, sed iaculis urna. Suspendisse scelerisque lacinia purus eget porta. Cras eget tempor lorem.",
                            Name = "Grizmann",
                            Email = "grizzy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Mauris mollis auctor velit, vitae scelerisque mauris feugiat sed. Mauris eget cursus tortor. Curabitur a dui in turpis consectetur gravida",
                            Name = "Sandra",
                            Email = "sandy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Aenean vel egestas dolor. Phasellus ac dictum tellus. Nam vel ante sit amet velit finibus finibus quis ut nisi. Mauris et varius est. Mauris vel efficitur odio. Duis faucibus nulla lacus, vel feugiat dui consectetur et.",
                            Name = "Saul goodman",
                            Email = "saulG@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Duis aliquet, enim lobortis ultricies euismod, risus neque tincidunt risus, in vestibulum erat nunc vitae nisl. In congue erat vel ultricies egestas. Vestibulum placerat, elit non condimentum maximus, sapien ligula porttitor orci, at pellentesque est velit non est. Ut efficitur ligula magna, id laoreet est faucibus non.",
                            Name = "Kendel",
                            Email = "adam@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = " Donec fermentum lacus et lacus pharetra, ut molestie justo semper. Curabitur id ultricies tortor. Fusce quis elit sit amet velit vulputate molestie. Vivamus at euismod ex. Etiam sit amet porttitor ligula.",
                            Name = "Jet Victor",
                            Email = "jv@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Duis aliquet, enim lobortis ultricies euismod, risus neque tincidunt risus, in vestibulum erat nunc vitae nisl. In congue erat vel ultricies egestas. Vestibulum placerat, elit non condimentum maximus, sapien ligula porttitor orci, at pellentesque est velit non est. Ut efficitur ligula magna, id laoreet est faucibus non.",
                            Name = "Walter white",
                            Email = "walter@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Aliquam euismod nisl imperdiet enim consectetur, eu dapibus massa placerat. Phasellus cursus vehicula mi, at tincidunt odio consectetur at. Integer neque turpis, sagittis vel ullamcorper imperdiet, ullamcorper quis dolor. Donec vitae rhoncus nunc. Maecenas placerat arcu in nisi viverra viverra.",
                            Name = "Dembele",
                            Email = "dd@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Nam tempus fringilla ligula, eget consequat dolor tincidunt id. Aliquam tincidunt, lectus ac hendrerit cursus, dui odio facilisis ligula, et placerat nisi leo quis nunc. Donec fermentum lacus et lacus pharetra, ut molestie justo semper. Curabitur id ultricies tortor. Fusce quis elit sit amet velit vulputate molestie. Vivamus at euismod ex. Etiam sit amet porttitor ligula.",
                            Name = "Suarez",
                            Email = "suarez@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Donec at ante a ante commodo consequat. Donec at cursus justo. Phasellus pretium elit sit amet enim placerat, non volutpat ligula sollicitudin. Curabitur a sem ac enim lacinia fringilla in nec nisl. Suspendisse potenti. Maecenas orci quam, eleifend eu metus at, placerat imperdiet elit. ",
                            Name = "Donald",
                            Email = "don@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Curabitur pellentesque elementum mauris, ac vulputate purus vehicula at. Pellentesque non tortor ornare, consectetur nisl et, hendrerit quam. Morbi mattis accumsan purus eget ornare. Donec interdum tristique nulla, rutrum posuere felis tincidunt et. Praesent suscipit et nulla nec scelerisque.",
                            Name = "Shayme",
                            Email = "sh@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(3,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = " Fusce vehicula lectus in ex aliquam sollicitudin. Nunc nec laoreet tellus. Mauris eu elit a nulla ultrices lobortis in condimentum mi. Curabitur porta nunc quis faucibus venenatis.",
                            Name = "torbido",
                            Email = "tr@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(1,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Phasellus eleifend pharetra urna non maximus. Nulla in pretium risus. Sed aliquet tempor felis ac faucibus. Nulla posuere elit nec erat elementum egestas. Aenean convallis ligula vulputate posuere egestas. Etiam vestibulum, lectus sit amet rhoncus facilisis, nunc arcu hendrerit neque, ac auctor eros velit ut nisi.",
                            Name = "Benzema",
                            Email = "bel@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Aenean id lobortis nulla. Phasellus pretium elit nec ullamcorper bibendum. Nullam bibendum erat a mi rhoncus iaculis eu sit amet mauris. Curabitur quam velit, sollicitudin at ultrices ut, mollis id augue. Nam dapibus facilisis sem. Nam malesuada nunc at est sollicitudin, a luctus sapien eleifend. Praesent mi purus",
                            Name = "Andrew",
                            Email = "andy@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Mauris vestibulum dolor non est rutrum, quis consectetur quam pharetra. Donec a elementum sem. Quisque at nibh condimentum, fringilla diam et, volutpat magna. Sed consectetur vitae mauris et rhoncus. Etiam eget odio dolor. Sed vel justo nec est pellentesque volutpat eget eget enim",
                            Name = "Lumi",
                            Email = "lucas@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Duis at blandit purus. Suspendisse potenti. Nam ac ornare ante. Aliquam posuere ultricies turpis et laoreet. Nam eleifend magna et nulla ultricies, sit amet fringilla ante varius. Donec blandit massa quam, nec finibus turpis dapibus id. Integer aliquet malesuada turpis eget euismod. ",
                            Name = "Jogn cena",
                            Email = "cena@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Pellentesque malesuada, est nec egestas vulputate, velit mauris vehicula velit,.",
                            Name = "Cynthia",
                            Email = "cynrthia@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(4,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Vestibulum tristique non magna gravida blandit. Duis consequat sodales massa ut sollicitudin. Donec imperdiet congue lectus eu mattis. Mauris pharetra blandit tincidunt. Aliquam erat mi, faucibus at tortor ut, elementum dictum leo. Proin nec volutpat dui.",
                            Name = "Lionel messi",
                            Email = "lmessi@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
                            Comment = "Etiam dictum nec lorem in dignissim. Nam et ex volutpat, ultrices augue vel, aliquam tortor. Nunc volutpat euismod tortor.",
                            Name = "Ethanla",
                            Email = "thye@gmail.com",
                            Product = product,
                            ProductId = product.Id,
                            Rating = rand.Next(5,6),
                        },

                        new Review{IsDeleted=false,DateCreated=DateTime.Now,DateModified=DateTime.Now,
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

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            ViewBag.ReviewCount = _reviewService.GetAllReviewCount();
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