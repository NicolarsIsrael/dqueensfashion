using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Utilities
{
    public class AppConstant
    {
        public const string ApplicationName = "DqueensFashion";
        public const string logoUrl = "https://houseofdqueens.com/Content/Images/HdqLogo.png";
        public const string AdminEmail = "admin@gmail.com";
        public const string AdminPassword = "Abc123*";
        public const string AdminRole = "Admin";
        public const string CustomerRole = "Customer";

        //email accounts
        public const string HDQ_INFO_MAIL_ACCOUNT = "info@houseofdqueens.com";
        public static readonly NetworkCredential HDQ_INFO_ACCOUNT_MAIL_CREDENTIALS = new NetworkCredential(
                                      ConfigurationManager.AppSettings["hdqInfoMailAccount"],
                                      ConfigurationManager.AppSettings["hdqInfoMailPassword"]
                                      );

        public const string HDQ_ADMIN_MAIL_ACCOUNT = "admin@houseofdqueens.com";
        public static readonly NetworkCredential HDQ_ADMIN_ACCOUNT_MAIL_CREDENTIALS = new NetworkCredential(
                              ConfigurationManager.AppSettings["hdqAdminMailAccount"],
                              ConfigurationManager.AppSettings["hdqAdminMailPassword"]
                              );

        public const string HDQFacebookLink = "https://facebook.com/hdq";
        public const string HDQTwitterLink = "https://twitter.com/hdq";
        public const string HDQInstagramLink = "https://instagram.com/hdq";
        public const string HDQYoutubeLink = "https://youtube.com/hdq";


        public const string DefaultProductImage = "https://image.freepik.com/free-vector/farmer-peasant-illustration-man-with-beard-spade-farmland_33099-575.jpg";
        public const int ProductIndexPageSize = 16;
        public const int ReviewsPageSize = 10;
        public const int HomeIndexProductCount = 16;

        public const string ProductImageBasePath = "~/Content/Images/Products/";
        public const string OutfitsName = "Outfits";
        public const int OutfitsId = 1;

        public static int MaxOutfitAddToCart = 50;

        //outfit measurements

        public static readonly double[] ShoulderValues = { 10, 10.5, 11, 11.5, 12, 12.5, 13, 13.5, 14,
                                                        14.5, 15, 15.5, 16, 16.5, 17, 17.5, 18, 18.5, 19, 19.5, 20 };
        public static readonly double[] ArmHoleValues = { 10, 10.5, 11, 11.5, 12, 12.5, 13, 13.5, 14,
                                                        14.5, 15, 15.5, 16, 16.5, 17, 17.5, 18, 18.5, 19, 19.5, 20 };
        public static readonly double[] BustValues = { 25, 25.5, 26, 26.5, 27, 27.5, 28, 28.5, 29, 29.5, 30, 30.5, 31, 31.5,
                                                        32, 32.5, 33, 33.5, 34, 34.5, 35, 35.5, 36, 36.5, 37, 37.5, 38, 38.5, 39, 39.5, 40 };
        public static readonly double[] WaistValues = { 20, 20.5, 21, 21.5, 22, 22.5, 23, 23.5, 24, 24.5, 25, 25.5, 26, 26.5, 27, 27.5, 28,
                                                        28.5, 29, 29.5, 30, 30.5, 31, 31.5, 32, 32.5, 33, 33.5, 34, 34.5, 35, 35.5, 36, 36.5, 37, 37.5, 38 };
        public static readonly double[] HipsValues = { 25, 25.5, 26, 26.5, 27, 27.5, 28, 28.5, 29, 29.5, 30, 30.5, 31, 31.5,
                                                        32, 32.5, 33, 33.5, 34, 34.5, 35, 35.5, 36, 36.5, 37, 37.5, 38, 38.5, 39, 39.5, 40, 40.5, 41, 41.5, 42 };
        public static readonly double[] ThighValues = {20, 20.5, 21, 21.5, 22, 22.5, 23, 23.5, 24, 24.5, 25, 25.5, 26, 26.5,
                                                        27, 27.5, 28, 28.5, 29, 29.5, 30, 30.5, 31, 31.5,32, 32.5, 33,};
        public static readonly double[] FullBodyValues = { 40, 40.5, 41, 41.5, 42, 42.5, 43, 43.5, 44, 44.5, 45, 45.5, 46, 46.5, 47, 47.5,
                                                          48, 48.5, 49, 49.5, 50, 51, 51.5, 52, 53, 53.5, 54, 54.5, 55, 55.5, 56, 56.5, 57, 47.5,
                                                            58, 58.5, 59, 59.5, 60, 61, 61.5, 62, 62.5, 63, 63.5, 64, 64.5, 65 };
        public static readonly double[] KneeGarmentLengthValues = { 25, 25.5, 26, 26.5, 27, 27.5, 28, 28.5, 29, 29.5, 30, 30.5, 31, 31.5,
                                                        32, 32.5, 33, 33.5, 34, 34.5, 35, 35.5, 36, 36.5, 37, 37.5, 38, 38.5, 39, 39.5, 40 };
        public static readonly double[] TopLengthValues = {20, 20.5, 21, 21.5, 22, 22.5, 23, 23.5, 24, 24.5, 25, 25.5, 26, 26.5,
                                                        27, 27.5, 28, 28.5, 29, 29.5, 30,  };
        public static readonly double[] TrousersLengthValues = { 30, 30.5, 31, 31.5, 32, 32.5, 33, 33.5, 34, 34.5, 35, 35.5, 36, 36.5, 37, 37.5,
                                                                38, 38.5, 39, 39.5, 40, 40.5, 41, 41.5, 42, 42.5, 43, 43.5, 44, 44.5, 45 };
        public static readonly double[] RoundAnkleValues = { 10, 10.5, 11, 11.5, 12, 12.5, 13, 13.5, 14,
                                                        14.5, 15, 15.5, 16, 16.5, 17, 17.5, 18, 18.5, 19, 19.5, 20 };
        public static readonly double[] NipNipValues = { 1, 1.5, 2, 2.5, 3, 3.5, 4,
                                                        4.5, 5, 5.5, 6, 6.5, 7, 7.5, 8, 8.5, 9, 9.5, 10, 10.5, 11, 11.5, 12, 12.5, 13, 13.5, 14, };
        public static readonly double[] SleevesLengthValues = {20, 20.5, 21, 21.5, 22, 22.5, 23, 23.5, 24, 24.5, 25, 25.5, 26, 26.5,
                                                        27, 27.5, 28, 28.5, 29, };


        //general values
        public const decimal HDQSubscriptionDiscount = 10;
        public const decimal ShippingPrice = 4.99M;
        public const int GeneralValId = 1;


        //frequent asked questions
        public static readonly string[,] faq = {
            { "Lorem Ipsum has been the industry's standard dummy text ever since the 1500s,",
                "Anim pariatur cliche reprehenderit, enim eiusmod high life accusamus terry richardson ad squid. Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident." },
            { "It is a long established fact that a reader will be distracted by the readable content",
                " it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy." },
            { "more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.","There are many variations of passages of Lorem Ipsum available, but the majority have suffered alteration in some form, by injected humour, or randomised words which don't look even slightly believable."},
            { "Sed laoreet arcu mauris, a condimentum ante pharetra id.",
                "Donec a neque dolor. Nam in pharetra ipsum. Fusce fringilla, massa ut ultrices tempor, nulla est bibendum massa, vitae interdum sapien velit id ante. Fusce mollis convallis odio, et aliquet neque euismod sit amet. Donec posuere odio eu vulputate congue. Praesent id feugiat augue. Nam at sapien aliquam, dapibus turpis ut, aliquam mi." },
            { "Nulla id quam in sem efficitur facilisis id semper nulla.",
                "Suspendisse posuere ornare sapien id ullamcorper. Pellentesque lacinia vehicula varius. Integer lectus mi, condimentum nec pretium ut, hendrerit eu nulla. Nam a suscipit nunc, ac dignissim nibh. Proin finibus lacinia nulla eu suscipit. Nulla ac mauris eu sapien cursus laoreet. Proin molestie viverra aliquet. Nam at tellus maximus, rhoncus dolor nec, consequat erat. Duis feugiat mauris at tellus aliquet, fringilla commodo risus pharetra." },
            { "Etiam volutpat ex id feugiat accumsan. Integer purus augue, tincidunt sed tempor ac,",
                "aliquam eget eros. Aliquam porta eros at tortor blandit, quis tincidunt odio rutrum. Donec a sodales urna. Morbi vitae velit elit. Aliquam at leo nulla. Integer non augue eu lorem accumsan faucibus. Donec et vehicula est. Aenean euismod diam in purus fringilla dictum." },
            { "Nulla imperdiet neque eu velit interdum porta.",
                "Curabitur aliquet congue arcu, non varius eros. Vestibulum sagittis ultricies sem, ac consectetur augue tincidunt id. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Sed sit amet justo id sapien auctor pulvinar eu ac leo. Vestibulum ac pretium arcu. Aenean sed elit posuere, fringilla tellus sed, cursus dui. Nullam sed urna rhoncus, fermentum metus ac, imperdiet urna. Proin eget rutrum enim, eget molestie lacus. Cras bibendum porta mattis. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos." },
            { "Suspendisse eget aliquam risus. Sed eleifend orci leo.",
                "Ut facilisis elementum erat ac elementum. Maecenas quam neque, maximus ut tellus dignissim, elementum commodo nulla. Nulla molestie, dui in iaculis ultricies, lectus enim blandit lorem, id pulvinar nibh elit vitae lacus. Praesent gravida porttitor semper. Nulla dapibus elit sit amet volutpat pulvinar. Nam vitae justo eget nisl rhoncus tempus. Suspendisse blandit nibh a velit faucibus condimentum." },
            { "Maecenas tincidunt, tortor sed consectetur consectetur",
                "nunc arcu vulputate eros, a sollicitudin risus dolor eu ligula. Nulla ante purus, elementum eu dui et, efficitur fringilla ex. Sed luctus erat vel dolor scelerisque, non porta leo mattis. Sed volutpat, libero ac ultrices pellentesque, sem elit rutrum orci, a faucibus metus diam pretium elit. Donec nibh leo, placerat vitae nisl ut, porta accumsan magna. Donec venenatis sagittis turpis in mollis. Proin orci ex, lobortis vel accumsan et, iaculis sit amet erat." },
            { "Phasellus laoreet, lorem sodales rhoncus feugiat, massa dolor varius purus",
                "ultrices nunc arcu porttitor augue. In nec velit ut arcu suscipit varius sit amet at massa. Vivamus lacinia, lacus eu congue interdum, turpis libero vehicula purus, at aliquam risus dui nec nisi. Aenean id viverra nunc. Vivamus et magna non enim blandit sollicitudin. Integer porta purus ut venenatis scelerisque. Duis aliquet ipsum at ligula tincidunt varius. Suspendisse in porta justo." }
        };

        //sort options
        public const int BestDeals = 1;
        public const int BestSelling = 2;
        public const int AlphabeticallyAZ = 3;
        public const int AlphabeticallyZA = 4;
        public const int PriceLowToHigh = 5;
        public const int PriceHighToLow = 6;
        public const int MostRecent = 7;
        public const int LeastRecent = 8;
        public const int HighestRating = 9;
        public const int LowestRating = 10;

        //public static readonly string[,] SortOptions = {
        //     {"BestDeals","1","How"},
        //     {"Np","2","Yes"}
        //    };

        //orange #E97128
        //deep blue #212529
        //gold #df7204
        //gold hover: #ff8d19
        //purple #96157a
        //purple hover #9c3b8d

        //dark #262626 //might use later


        //nav bar former color:#0a0c13
    }
}
