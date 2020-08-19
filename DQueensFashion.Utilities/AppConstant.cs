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

        public const string HDQ_MESSAGE_MAIL_ACCOUNT = "message@houseofdqueens.com";
        public static readonly NetworkCredential HDQ_MESSAGE_ACCOUNT_MAIL_CREDENTIALS = new NetworkCredential(
                              ConfigurationManager.AppSettings["hdqMessageMailAccount"],
                              ConfigurationManager.AppSettings["hdqMessageMailPassword"]
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
        public const int ProductIndexPageSize = 40;
        public const int ReviewsPageSize = 10;
        public const int HomeIndexProductCount = 16;

        public const string ProductImageBasePath = "~/Content/Images/Products/";
        public const string OutfitSampleImageBasePath = "~/Content/Images/OutfitSamples/";

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
        public const decimal MinFreeShippingPrice = 35.00M;
        public const int GeneralValId = 1;


        //frequent asked questions
        public static readonly string[,] faq = {
            { "What  is African print fabric?",
                "African fabrics have beautiful bright colors, idiosyncratic designs and patterns which give  a sense of a rich heritage and cultural meaning.\r\n\r\n African print fabrics are worn for special occasions such as family reunions, weddings, and events. It can also be used for various accessories such as head wraps, face mask, bags, shoes, decoration,  dresses shirt, and more. African fabric forms part of a cultural identity and cultural heritage.\r\n\r\n We stock up on  high quality African print fabrics and we have a wide range of varieties. We have largest collections of African fabrics  like African wax, hitarget, african kente, etc. Our fabrics are sold wholesale price." },

            { "Do you have a physical store?",
                "Houseofdqueens is basically an online store with a  warehouse distribution facility. We do not have a traditional brick and mortar store." },

            { "Do you have a catalog?",
                "Since our inventory changes often, because of this , we do not offer a printed catalog. All of our fabrics can be found on our website with a picture, price, and description." },

            { "Do you have samples?",
                "We do sell samples of most of our fabrics. Look for the 'Buy Sample' button associated with each fabric. But kindly note that many of the fabrics we stock are not re-orderable, it is possible that a fabric could sell out by the time you receive your sample." },

            { "I need more quantity than your website has in stock?",
                "If you need more than the available quantity we have on the website in a large quantity kindly contact us via our email." },

            { "Can I order multiple cut for same fabrics?",
                "No. If multiple cuts are required for the same fabric, individual orders must be placed." },

            { "What is the minimun yardage I can have?",
                "Minimum cut length is 1 yard. " },

            { "What is 1 yard?",
                "1 yard is Between  34inches- 36inches  by width And Between 44inches to 46inches by length" },

            { "Apart from African print fabrics, what other items do you have?",
                "All our availabile items are on the website which includes African lace fabrics, silk materials,  headwraps and other african accessories." },
        };

        public static readonly string[,] faqPurchasingSupport = {
            { "What are you payment method?",
                "We accept PayPal\r\n Always include your name" },

            { "After payment what  next?",
                "We will confirm your payment from our account \r\nWe will send you a confirmation number " +
                    "\r\n We will then process ur item and send you tracking number for your item." },

            
        };

        public static readonly string[,] faqShippingReturn = {
            { "Do you ship internationally?",
                "Yes, North America and Europe." },

            { "Shipping information.",
                "Kindly fill in your correct address including zip code and your full name " },

            { "When can I expect my order?",
                "Estimated delivery is Between 3-7 business days. We use USPS and UPS for shipping your item within USA." +
                    "\r\nWe also use usps for international delivery but the time frame is usually Between 2-6 weeks as we are not in control of custom policies." },

            { "What is your return policy?",
                "We take serious measure while packing right product and color which you have asked for. We make sure we are sending right product and color and complete new and intake product. However if there is any issue about order we request you to contact us  via our emal with 7days." },


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
