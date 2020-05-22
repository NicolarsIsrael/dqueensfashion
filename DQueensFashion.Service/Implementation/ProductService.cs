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
    public class ProductService : IProductService
    {
        IUnitOfWork uow;

        public ProductService(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        public void AddProduct(Product product)
        {
            if (product == null)
                throw new Exception();

            uow.ProductRepo.Add(product);
            uow.Save();
        }

        public int GetAllProductsCount()
        {
            return uow.ProductRepo.GetAll().Count();
        }
    }

}
