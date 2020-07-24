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
            int count = 0;
            var requests = uow.RequestRepo.GetAll().ToList();
            foreach(var req in requests)
            {
                var product = uow.ProductRepo.GetProductByIdWithRelationships(req.ProductId);
                if (product != null)
                    count++;
            };
            return count;
        }

        public IEnumerable<Request> GetAllRequests()
        {
            return uow.RequestRepo.GetAll();
        }

        public IEnumerable<Request> GetAllRequestsForProduct(int productId)
        {
            return uow.RequestRepo.GetAll().Where(p => p.ProductId == productId).ToList();
        }

        public void DeleteRequestsRange(IEnumerable<Request> requests)
        {
            uow.RequestRepo.DeleteRangeFromDb(requests);
            uow.Save();
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
