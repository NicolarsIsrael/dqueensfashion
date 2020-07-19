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
    public class RequestService : IRequestService
    {
        IUnitOfWork uow;
        public RequestService(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        public int GetRequestCount()
        {
            return uow.RequestRepo.Count();
        }

        public void AddRequest(Request request)
        {
            if (!ValidateRequestDetails(request))
                throw new Exception();

            uow.RequestRepo.Add(request);
            uow.Save();
        }

        public int GetTotalRequests()
        {
            return uow.RequestRepo.GetAll().Sum(r => r.Quantity);
        }

        public IEnumerable<Request> GetAllRequests()
        {
            return uow.RequestRepo.GetAll();
        }

        private bool ValidateRequestDetails(Request request)
        {
            if (request == null)
                return false;

            if (string.IsNullOrEmpty(request.CustomerEmail))
                return false;

            if (uow.ProductRepo.GetProductByIdWithRelationships(request.ProductId)==null)
                return false;

            if (request.Quantity < 1)
                return false;

            return true;
        }

    }
}
