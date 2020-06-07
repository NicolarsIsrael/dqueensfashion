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
    public class ReviewService : IReviewService
    {
        IUnitOfWork uow;
        public ReviewService(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        public void AddReview(Review review)
        {
            if (!ValidateReviewDetails(review))
                throw new Exception();

            uow.ReviewRepo.Add(review);
            uow.Save();
        }

        public int GetAllReviewCount()
        {
            return uow.ReviewRepo.Count();
        }

        public IEnumerable<Review> GetAllReviewsForProduct(int productId)
        {
            return uow.ReviewRepo.GetAllReviewsWithRelationships().Where(r => r.ProductId == productId).ToList();
        }

        public double GetAverageRating(int productId)
        {
            int TotalRating = uow.ReviewRepo.GetAllReviewsWithRelationships().Where(r => r.ProductId == productId).Sum(r => r.Rating);
            int TotalReviewCount = uow.ReviewRepo.GetAllReviewsWithRelationships().Where(r => r.ProductId == productId).Count();

            if (TotalReviewCount < 1)
                return 0;

            double averageRating = TotalRating / (double)TotalReviewCount;
            return averageRating;

        }

        public int GetReviewCountForProduct(int productId)
        {
            return uow.ReviewRepo.GetAllReviewsWithRelationships().Where(r => r.ProductId == productId).Count();
        }

        private bool ValidateReviewDetails(Review review)
        {
            if (review == null)
                return false;

            if (review.Product == null)
                return false;

            if (string.IsNullOrEmpty(review.Name) || string.IsNullOrWhiteSpace(review.Name) || !char.IsLetterOrDigit(review.Name[0]))
                return false;

            if (string.IsNullOrEmpty(review.Comment) || string.IsNullOrWhiteSpace(review.Comment))
                return false;

            if (review.Rating < 1 || review.Rating > 5)
                return false;

            return true;
        }

    }
}
