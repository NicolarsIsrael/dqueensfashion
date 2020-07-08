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

        public const string AdminEmail = "admin@gmail.com";
        public const string AdminPassword = "Abc123*";
        public const string AdminRole = "Admin";
        public const string EmployeeRole = "Employee";
        public const string CustomerRole = "Customer";

        public const string HDQ_EMAIL_ACCOUNT = "bakarenicolarsisrael@gmail.com";
        public const string HDQ_EMAIL_TITLE = "HDQ";

        public static readonly NetworkCredential MAIL_CREDENTIALS = new NetworkCredential(
                                      ConfigurationManager.AppSettings["mailAccount"],
                                      ConfigurationManager.AppSettings["mailPassword"]
                                      );

        public const string DefaultProductImage = "https://image.freepik.com/free-vector/farmer-peasant-illustration-man-with-beard-spade-farmland_33099-575.jpg";
        public const int ProductIndexPageSize = 20;
        public const int ReviewsPageSize = 10;
        public const int HomeIndexProductCount = 16;

        public const string ProductImageBasePath = "~/Content/Images/Products/";
        public const int CustomMadeCategoryId = 4;
        public const string CustomMadeName = "Custom-Made";
        public const int ReadyMadeCategoryId = 3;
        public const string ReadyMadeName = "Ready-Made";

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

        public static readonly string[] ReadyMadeSizes = { "Extra small (XS)", "Small (S)", "Medium (M)", "Large (L)", "Extra large (XL)" };
        public static int MaxCustomMadeAddToCart = 50;

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
        public const string HDQAddress = "123 Street, Old Trafford, New South London , UK";
        public const string HDQEmail = "Hdq@gmail.com";
        public const string HDQPhoneNumber = "+45 637 872";
        public const decimal HDQSubscriptionDiscount = 10;
        public const int GeneralValId = 1;


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
