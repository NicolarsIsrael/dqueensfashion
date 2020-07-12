using DQueensFashion.Data.Contract;
using DQueensFashion.Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQueensFashion.Service.Implementation
{
    public class MessageService : IMessageService
    {
        IUnitOfWork uow;
        public MessageService(IUnitOfWork _uow)
        {
            uow = _uow;
        }

        public int GetMessageCount()
        {
            return uow.MessageRepo.Count();
        }
    }
}
