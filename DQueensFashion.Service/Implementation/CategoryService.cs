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
            if (category == null)
                throw new Exception();

            uow.CategoryRepo.Add(category);
            uow.Save();
        }

        public int GetAllCategoriesCount()
        {
            return uow.CategoryRepo.Count();
        }
    }
}
