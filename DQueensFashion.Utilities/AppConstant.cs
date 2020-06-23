using System;
using System.Collections.Generic;
using System.Linq;
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

        public const string DefaultProductImage = "https://image.freepik.com/free-vector/farmer-peasant-illustration-man-with-beard-spade-farmland_33099-575.jpg";
        public const int ProductIndexPageSize = 20;
        public const int ReviewsPageSize = 10;
        public const int HomeIndexProductCount = 16;

        public const string ProductImageBasePath = "~/Content/Images/Products/";
        public const int CustomMadeCategoryId = 4;
        public const int ReadyMadeCategoryId = 3;

        public static readonly string[] ReadyMadeSizes = { "Extra small (XS)", "Small (S)", "Medium (M)", "Large (L)", "Extra large (XL)" };

        public static readonly int[] BurstValues = { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27 };
        public static readonly int[] ShoulderValues = { 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113, 114, 115, 116, 117 };
        public static readonly int[] WaistValues = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };

        //orange #E97128
        //deep blue #212529
    }
}
