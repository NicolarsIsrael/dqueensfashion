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

        public void RemoveFromMailingList(string email)
        {
            var m = uow.MailingListRepo.GetAll().Where(_m => _m.EmailAddress == email).FirstOrDefault();
            if (m != null)
                uow.MailingListRepo.DeleteFromDb(m);
            uow.Save();
        }

        public bool CheckIfSubscribed(int customerId)
        {
            Customer customer = uow.CustomerRepo.Get(customerId);
            if (customer == null)
                throw new Exception();

            MailingList mailingList = uow.MailingListRepo.GetAll()
                .Where(m => m.EmailAddress == customer.Email).FirstOrDefault();

            if (mailingList == null)
                return false;

            return true;
        }
    }
}
