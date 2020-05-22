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

        public int GetAllProductsCount()
        {
            return uow.ProductRepo.GetAll().Count();
        }
    }

}
