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
    public class WishListService : IWishListService
    {
        IUnitOfWork uow;

        public WishListService(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        public void AddWishList(WishList wishList)
        {
            if (!ValidateWishListDetails(wishList))
                throw new Exception();

            if (!CheckIfProductExistInCustomerWishList(wishList.ProductId, wishList.CustomerId))
            {
                uow.WishListRepo.Add(wishList);
                uow.Save();
            }
            
        }

        public int GetAllWishListCount()
        {
            return uow.WishListRepo.Count();
        }

        private bool CheckIfProductExistInCustomerWishList(int productId, int customerId)
        {
            if (uow.WishListRepo.GetAll().Where(w => w.ProductId == productId && w.CustomerId == customerId).Count() > 0)
                return true;
            return false;
        }

        private bool ValidateWishListDetails(WishList wishList)
        {
            if (wishList == null)
                return false;

            if (string.IsNullOrEmpty(wishList.ProductName))
                return false;

            if (wishList.CustomerId < 0 || wishList.ProductId < 0)
                return false;

            return true;
        }
    }
}

