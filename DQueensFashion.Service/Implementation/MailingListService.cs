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

        public void AddToMailingList(MailingList mailingList)
        {
            var m = uow.MailingListRepo.GetAll().Where(_m => _m.EmailAddress == mailingList.EmailAddress).FirstOrDefault();
            if (m == null)
            {
                uow.MailingListRepo.Add(mailingList);
                uow.Save();
            }

        }
    }
}
