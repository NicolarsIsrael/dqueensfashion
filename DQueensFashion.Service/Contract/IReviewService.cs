using DQueensFashion.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Service.Contract
{
    public interface IReviewService
    {
        int GetAllReviewCount();
        void AddReview(Review review);
        double GetAverageRating(int productId);
        IEnumerable<Review> GetAllReviewsForProduct(int productId);
        int GetReviewCountForProduct(int productId);
        void AddRangeReveiew(IEnumerable<Review> reviews);
        bool CanReview(int lineItemId);
        IEnumerable<LineItem> GetPendingReviews(int customerId);
    }
}
