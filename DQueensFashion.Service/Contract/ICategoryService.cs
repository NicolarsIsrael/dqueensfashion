using DQueensFashion.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Service.Contract
{
    public interface ICategoryService
    {
        void AddCategory(Category category);
        void UpdateCategory(Category category);
        int GetAllCategoriesCount();
        IEnumerable<Category> GetAllCategories();
        Category GetCategoryByName(string categoryName);
        Category GetCategoryById(int id);

    }
}
