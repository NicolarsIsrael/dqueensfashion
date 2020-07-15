﻿using DQueensFashion.Core.Model;
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
            if (!ValidateProductDetails(product))
                throw new Exception();

            uow.ProductRepo.Add(product);
            uow.Save();
        }

        public void UpdateProduct(Product product)
        {
            //if (!ValidateProductDetails(product))
            //    throw new Exception();

            uow.ProductRepo.Update(product);
            uow.Save();
        }

        public int GetAllProductsCount()
        {
            return uow.ProductRepo.GetAll().Count();
        }

        public IEnumerable<Product> GetAllProductsWithDelete()
        {
            return uow.ProductRepo.GetAllWithDelete();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return uow.ProductRepo.GetAllProductsWithRelationships()
                .Where(p => p.Quantity > 0);
        }

        public IEnumerable<Product> GetAllProductsIncludingLowQuantity()
        {
            return uow.ProductRepo.GetAllProductsWithRelationships();
        }

        public IEnumerable<Product> GetAllProductsForCategory(int categoryId)
        {
            return GetAllProducts().Where(p => p.CategoryId == categoryId)
                .Where(p => p.Quantity > 0);
        }

        public IEnumerable<Product> GetRelatedProducts(int productId,int categoryId)
        {
            return GetAllProducts().Where(p => p.Id!=productId && p.CategoryId == categoryId)
                .Where(p => p.Quantity > 0);
        }


        public Product GetProductById(int id)
        {
            return uow.ProductRepo.GetProductByIdWithRelationships(id);
        }

        public decimal CalculateProductPrice(decimal price, decimal discount)
        {
            decimal p = price * (1 - (discount / (decimal)100));
            p= Math.Round(p, 2,MidpointRounding.AwayFromZero);
            return p;
        }

        public bool CheckIfProductIsNew(DateTime datecreated)
        {
            if (datecreated.AddMonths(1) > DateTime.Today)
                return true;
            return false;
        }

        private bool ValidateProductDetails(Product product)
        {
            if (product == null )
                return false;

            if (product.Category == null)
                return false;

            if (string.IsNullOrEmpty(product.Name) || string.IsNullOrWhiteSpace(product.Name)
               || product.Name.Length > 50 || product.Name.Length < 2 || !char.IsLetterOrDigit(product.Name[0]))
                return false;

            if (string.IsNullOrEmpty(product.Description) || string.IsNullOrWhiteSpace(product.Description)
              || product.Description.Length > 10000 || product.Description.Length < 2 || !char.IsLetterOrDigit(product.Description[0]))
                return false;

            if (product.Quantity < 1)
                return false;

            if (product.Price < 0)
                return false;

            if (product.Discount < 0 || product.Discount > 100)
                return false;

            if (product.SubTotal < 0 || product.SubTotal > product.Price)
                return false;

            if (product.Tags == null)
                return false;

            if (product.DeliveryDaysDuration < 1)
                return false;

            return true;
        }

    }

}
