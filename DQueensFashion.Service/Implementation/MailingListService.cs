using DQueensFashion.Data.Contract;
using DQueensFashion.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Service.Implementation
{
    public class MailingListService : IMailingListService
    {
        IUnitOfWork uow;
        public MailingListService(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        public int MailingListCount()
        {
            return uow.MailingListRepo.Count();
        }
    }
}
