using DQueensFashion.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Service.Contract
{
    public interface IProductService
    {
        int GetAllProductsCount();
        Product GetProductById(int id);
        void AddProduct(Product product);
        void UpdateProduct(Product product);
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetRelatedProducts(int productId, int categoryId);
        decimal CalculateProductPrice(decimal price, decimal discount);
        IEnumerable<Product> GetAllProductsWithDelete();
        bool CheckIfProductIsNew(DateTime datecreated);
    }
}
