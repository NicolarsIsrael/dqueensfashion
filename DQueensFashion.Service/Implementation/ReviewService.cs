using DQueensFashion.Data.Contract;
using DQueensFashion.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Service.Implementation
{
    public class ReviewService : IReviewService
    {
        IUnitOfWork uow;
        public ReviewService(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        public int GetAllReviewCount()
        {
            return uow.ReviewRepo.Count();
        }
    }
}
