using DQueensFashion.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Service.Contract
{
    public interface IRequestService
    {
        int GetRequestCount();
        void AddRequest(Request request);
        int GetTotalRequests();
        IEnumerable<Request> GetAllRequests();
        IEnumerable<Request> GetAllRequestsForProduct(int productId);
        void DeleteRequestsRange(IEnumerable<Request> requests);
    }
}
