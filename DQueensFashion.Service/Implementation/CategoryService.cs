using DQueensFashion.Core.Model;
using DQueensFashion.Data.Contract;
using DQueensFashion.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Service.Implementation
{
    public class CategoryService : ICategoryService
    {
        IUnitOfWork uow;
        public CategoryService(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        public void AddCategory(Category category)
        {
            try
            {
                if (!ValidateCategoryDetails(category))
                    throw new Exception();

                uow.CategoryRepo.Add(category);
                uow.Save();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateCategory(Category category)
        {
            try
            {
                if (!ValidateCategoryDetails(category))
                    throw new Exception();

                uow.CategoryRepo.Update(category);
                uow.Save();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetAllCategoriesCount()
        {
            return uow.CategoryRepo.Count();
        }
        public Category GetCategoryById(int id)
        {
            return uow.CategoryRepo.Get(id);
        }

        public Category GetCategoryByName(string categoryName)
        {
            return uow.CategoryRepo.GetAll().Where(c => string.Compare(c.Name, categoryName, true) == 0).FirstOrDefault();
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return uow.CategoryRepo.GetAll().ToList();
        }

        private bool ValidateCategoryDetails(Category category)
        {
            if (category == null)
                return false;

            if (string.IsNullOrEmpty(category.Name) || string.IsNullOrWhiteSpace(category.Name)
               || category.Name.Length > 50 || category.Name.Length < 2 || !char.IsLetter(category.Name[0]))
                return false;

            return true;
        }
        
    }
}
