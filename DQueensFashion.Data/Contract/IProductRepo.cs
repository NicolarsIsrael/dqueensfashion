﻿using DQueensFashion.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Data.Contract
{
    public interface IProductRepo : ICoreRepo<Product>
    {
        Product GetProductByIdWithRelationships(int productId);
        IEnumerable<Product> GetAllProductsWithRelationships();
    }

}
