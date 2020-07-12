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

        public IEnumerable<Message> GetMessages()
        {
            return uow.MessageRepo.GetAll();
        }

        public Message GetMessageById(int id)
        {
            return uow.MessageRepo.Get(id);
        }

        public void AddMessage(Message message)
        {
            if (!ValidateMessage(message))
                throw new Exception();

            uow.MessageRepo.Add(message);
            uow.Save();
        }

        public void UpdateMessage(Message message)
        {
            if (!ValidateMessage(message))
                throw new Exception();

            uow.MessageRepo.Update(message);
            uow.Save();
        }

        private bool ValidateMessage(Message message)
        {
            if (message == null)
                return false;

            if (string.IsNullOrEmpty(message.Fullname) || string.IsNullOrWhiteSpace(message.Fullname)
                || !char.IsLetterOrDigit(message.Fullname[0]))
                return false;

            if (string.IsNullOrEmpty(message.Subject) || string.IsNullOrWhiteSpace(message.Subject))
                return false;

            if (string.IsNullOrEmpty(message.MessageSummary) || string.IsNullOrWhiteSpace(message.MessageSummary))
                return false;

            return true;
        }

    }
}
