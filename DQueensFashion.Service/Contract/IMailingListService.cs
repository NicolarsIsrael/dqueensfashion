using DQueensFashion.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Service.Contract
{
    public interface IMailingListService
    {
        int MailingListCount();
        void AddToMailingList(MailingList mailingList);
        bool CheckIfSubscribed(int customerId);
        void RemoveFromMailingList(string email);
    }
}
