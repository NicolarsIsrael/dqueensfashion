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
    }
}
