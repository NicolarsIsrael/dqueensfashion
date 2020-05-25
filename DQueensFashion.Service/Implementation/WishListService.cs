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

        public int GetAllWishListCount()
        {
            return uow.WishListRepo.Count();
        }
    }
}

