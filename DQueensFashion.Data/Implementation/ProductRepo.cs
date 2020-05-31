using DQueensFashion.Core.Model;
using DQueensFashion.Data.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Data.Implementation
{
    public class ProductRepo : CoreRepo<Product>, IProductRepo
    {
        public ProductRepo(DbContext ctx):base(ctx)
        {

        }

        //other functions
        public IEnumerable<Product> GetAllProductsWithRelationships()
        {
            return _dbContext.Set<Product>()
                .Include(p => p.Category)
                .Where(p => !p.IsDeleted)
                .ToList();
        }

        public Product GetProductByIdWithRelationships(int productId)
        {
            return _dbContext.Set<Product>()
                .Include(p => p.Category)
                .Where(p => p.Id == productId)
                .Where(p => !p.IsDeleted)
                .FirstOrDefault();
        }
    }

}

